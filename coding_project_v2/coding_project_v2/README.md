# Console API App
The Application reads in a json file containing an array of services
- The project can be run with visual studio, and is set to read from the basic_endpoints.json file

## Services
Each Service has endpoints

## Endpoint
Each endpoint has responses. 

## Response
Can be a string value, or a regex pattern 

## Operation 1 [Attempted]
Convert json file to a generic/dynamic C# object and get the relevant fields that we need 
to query the resource urls.

## Operation 2 [Attempted]
For each enabled service and resource url/endpoint make a get request.
Deserialize the returned json response and compare the relevant values, to check if the match
with the local values. 

## Operation 3 (Bonus) [Outstanding]
Add the ability to implement a single set of reusable identinfiers. Understood to mean, adding the 
ability to dynamically pass in the json file to use at runtime

## Operation 4 (Bonus) [Outstanding]
Implement content negotiation, by dynamic adding the request type to be either, json or xml

# Struggles
I struggled a lot with:
- converting the json to c# objects
- accessing the response, elements 
- comparing the local json values with the returned json values
- not all the regex fields where valid regex
- didn't know what to do with the "edge case" json data
