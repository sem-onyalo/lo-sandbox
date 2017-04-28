FORMAT: 1A

# LoyaltyOneText

Loyalty One Text Application for Developer Homework Assignment

# Group Text

Resources related to texts in the API.

## Text Collection [/v1/texts/{name}]

A Text response object has the following attributes:

+ Id (text unique identifier)
+ ParentId (the id of this text objects parent)
+ Text (the text value)
+ City (the city of the text originator)
+ Lat (the latitude of the city)
+ Lon (the longitude of the city)
+ Temp (the temperature of the city)

### Get Text Collection by Poster's Name [GET]

+ Response 200 (application/json)

        {
            "Name": "Joe",
            "Texts":  
            [
                {
                    "Id": 1,
                    "ParentId": 0,
                    "Text": "Favourite food?",
                    "City": "Toronto",
                    "Lat": "43.61",
                    "Lon": "-79.38",
                    "Temp": 24
                },
                {
                    "Id": 2,
                    "ParentId": 1,
                    "Text": "bread",
                    "City": "Paris",
                    "Lat": "48.85",
                    "Lon": "2.35",
                    "Temp": 18
                },
                {
                    "Id": 2,
                    "ParentId": 1,
                    "Text": "beans",
                    "City": "London",
                    "Lat": "51.50",
                    "Lon": "-0.13",
                    "Temp": 12
                }
            ],
            "Error": null
        }
        
## Text [/v1/text]

This request allows you to create a text.

A text request object has the following attributes:

+ Name (the name of the poster)
+ Text (the text value)
+ City (the city of the poster)
+ ParentId (the id of this text objects parent)

### Post Text [POST]

+ Response 200 (application/json)

        {
            "Name": "Joe",
            "Data":  
            {
                "Id": 1,
                "ParentId": 0,
                "Text": "Favourite food?",
                "City": "Toronto",
                "Lat": "43.61",
                "Lon": "-79.38",
                "Temp": 24
            },
            "Error": null
        }
