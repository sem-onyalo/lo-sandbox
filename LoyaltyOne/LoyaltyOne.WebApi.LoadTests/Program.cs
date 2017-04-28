using LoyaltyOne.WebApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyOne.WebApi.LoadTests
{
    class Program
    {
        static List<Task> threads = new List<Task>();
        static ConcurrentQueue<TextLoadTestData> dataInQueue = new ConcurrentQueue<TextLoadTestData>();
        static ConcurrentBag<long> elapsedMilliseconds = new ConcurrentBag<long>();

        static void Main(string[] args)
        {
            try
            {
                TextLoadTest loadTest = InitLoadTest(args);

                RunLoadTest(loadTest);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}", ex.Message);
            }

            Console.Write("\nAny key to exit");
            Console.ReadKey();
        }

        static TextLoadTest InitLoadTest(string[] args)
        {
            TextLoadTest loadTest = new TextLoadTest();
            loadTest.Name = args[0];

            if (File.Exists(string.Format("{0}\\{1}", Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase), string.Format("Tests\\{0}\\test-results.txt", loadTest.Name))))
                File.Delete(string.Format("{0}\\{1}", Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase), string.Format("Tests\\{0}\\test-results.txt", loadTest.Name)));

            Uri testParamsUri = new Uri(string.Format("{0}\\{1}", Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase), string.Format("Tests\\{0}\\test-params.txt", loadTest.Name)));
            string[] testParams = File.ReadAllLines(testParamsUri.LocalPath);
            loadTest.Threads = Convert.ToInt32(testParams[0]);
            loadTest.ApiUri = testParams[1];

            Uri testDataUri = new Uri(string.Format("{0}\\{1}", Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase), string.Format("Tests\\{0}\\test-data.json", loadTest.Name)));
            string testData = File.ReadAllText(testDataUri.LocalPath);
            loadTest.Data = JsonConvert.DeserializeObject<List<TextLoadTestData>>(testData);

            return loadTest;
        }

        static void RunLoadTest(TextLoadTest loadTest)
        {
            Console.WriteLine("Loading data...\n");
            foreach (TextLoadTestData d in loadTest.Data)
            {
                dataInQueue.Enqueue(d);
            }

            Console.WriteLine("Spawning threads...\n");
            for (int i = 0; i < loadTest.Threads; i++)
            {
                threads.Add(Task.Run(() => ThreadTextPostRequests(loadTest)));
            }

            Task.WaitAll(threads.ToArray());

            Console.WriteLine("\nTest results:\n");
            Console.WriteLine("Threads: {0}", loadTest.Threads);
            Console.WriteLine("Min: {0}", elapsedMilliseconds.Min());
            Console.WriteLine("Max: {0}", elapsedMilliseconds.Max());
            Console.WriteLine("Avg: {0}", elapsedMilliseconds.Average());
        }

        static void ThreadTextPostRequests(TextLoadTest loadTest)
        {
            TextLoadTestData data;
            while (dataInQueue.TryDequeue(out data))
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();

                string content = string.Format("{{ \"Name\": \"{0}\", \"Text\": \"{1}\", \"City\": \"{2}\", \"ParentId\": 0 }}", data.name, data.text, data.city);

                PostTextResponse response = PostRequest<PostTextResponse>(string.Format("{0}/v1/text", loadTest.ApiUri), "application/json", content);

                sw.Stop();

                elapsedMilliseconds.Add(sw.ElapsedMilliseconds);

                string responseSerialized = JsonConvert.SerializeObject(response.Data, Formatting.None);
                Console.WriteLine(responseSerialized);
            }
        }

        static T PostRequest<T>(string uri, string contentType, string content)
        {
            using (HttpClient client = new HttpClient())
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, uri);
                request.Content = new StringContent(content, Encoding.UTF8, contentType);

                Task<HttpResponseMessage> httpReponse = client.SendAsync(request);
                httpReponse.Wait();

                Task<string> responseContent = httpReponse.Result.Content.ReadAsStringAsync();
                responseContent.Wait();

                return JsonConvert.DeserializeObject<T>(responseContent.Result);
            }
        }
    }
}
