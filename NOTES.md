## 1. How I understand the task:

- read json file containing traffic events (maybe more than one).

- validate each event.

- keep only the latest one based on timestamp but do it smart with the best complexity.

- generate a cleaned output file and a short summary.

## 2. My approch:

- A service FileHandlerService.cs to read and parse the JSON into a list of events and to create the output file.

- A service EventsProcessor do the business logic by the function ValidateEvent and create the cleaned_json structure correctly.

## 3. My challenges or tradeoffs:

**json can have more than 1 event**.

-Used JsonDocument to detect if the root is an object or array and normalize it to a List<TrafficEvent>.


**handling duplicate traffic events in efficient within 60 seconds**.

To solve this, I used a dictionary data structure to group events by a composite key.  
- The key was created using concatenate of road_segment_id, event_type, and the timestamp rounded to the nearest second/minute .  
- This way, if two events occurred within the same time window, they would map to the same key.  
- When duplicates were detected, I kept the latest event based on the timestamp.

**caluclating the unique valid events**

There was a mistake with the initial calulation of the unique events so I decided to extend the dictionary value to tuple value that contains both the event and the number of times I got it .
I used it to minimize the changes I had to make in my current solution.

## Improvements

I would have added automated unit tests, more logs to document success points and failures in more details, handle any edge cases relevant to non-valid jsons

## 4. Optional extenstions
Aggregation - I used the same approach within the EventsProccessor services and while I first proccessed each event I also inserted it to different dictionarty which contains the current unique values per road_segement_id
In order to create a unique event types values I used Set stucture

API - 

## aditiional notes

I used chat GPT for syntax correction, create a stuctured project and basic error handling addition. 

