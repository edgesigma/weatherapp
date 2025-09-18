## weather app dev notes

### milestone one
validate vscode .NET dev configuration
create a console app
send GET request to hard-coded national weather svc. API
print JSON response to the console

### milestone two

while LatLong = null
    setAddress() //prompts user
    make Get request to geolocation API

    if..
        user submitted address returns a single result
            storeLatLong() // populates LatLong with a string
        user submitted address returns multiple results
            disambiguateResults()
            storeLatLong() // populates LatLong with a string


make subsequent GET request to the national weather svc. API
print JSON response to the console

## milestone three
refactor to OOP
add unit testing
design custom API endpoints to support frontend