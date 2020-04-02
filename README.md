# iTrellisTestProject
Shhhhh this is a secret

A few things to note here:
- Initial plan was to use Vue.js for the front end, pull in data via API routes only. I ended up spending way too long trying (and failing) to get Vue running, so I scrapped it and built the front end using Razor templates, instead. API routes are still used in the tests to check data.
- In lieu of a traditional database (and since there's not a complete CRUD here), the models are hydrated directly from JSON files using static repository classes. As a result, any changes made to the models aren't saved, as it would involve re-serializing the data to json; it isn't difficult to do, but (again), since there are no update/delete methods, it's not necessary.
- Test project is also homebrewed, setting up xUnit was taking forever, and I think the logic behind the tests is more important than the implementation (at least for right now)

I think that's about it!
