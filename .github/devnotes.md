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
finalize frontend UX (views & components)
design custom API endpoints to support frontend
create API application
integrate ASP.NET asset pipeline
serve hello-world SPA with dev server

### regional data
The main page design includes a carousel/list of cards, each featuring a given city. includes the city name, the region, state name and an iconic image

#### Northeast
- Maine – Acadia National Park
- New Hampshire – Mount Washington
- Vermont – Ben & Jerry’s Factory / Green Mountains
- Massachusetts – Freedom Trail (Boston)
- Rhode Island – The Breakers (Newport Mansions)
- Connecticut – Mystic Seaport
- New York – Statue of Liberty
- New Jersey – Atlantic City Boardwalk
- Pennsylvania – Liberty Bell & Independence Hall
- Delaware – Dover’s First State Heritage Park

#### Southeast
- Maryland – Fort McHenry (Baltimore)
- Virginia – Monticello (Thomas Jefferson’s home)
- West Virginia – Harpers Ferry
- North Carolina – Biltmore Estate (Asheville)
- South Carolina – Fort Sumter
- Georgia – Stone Mountain
- Florida – Everglades National Park / Kennedy Space 

#### Center
- Kentucky – Mammoth Cave National Park
- Tennessee – Graceland (Memphis)
- Alabama – USS Alabama Battleship Memorial Park
- Mississippi – Vicksburg National Military Park
- Arkansas – Hot Springs National Park

#### Midwest
- Ohio – Rock & Roll Hall of Fame (Cleveland)
- Michigan – Mackinac Island / Mackinac Bridge
- Indiana – Indianapolis Motor Speedway
- Illinois – Willis (Sears) Tower (Chicago)
- Wisconsin – House on the Rock
- Minnesota – Mall of America
- Iowa – Field of Dreams Movie Site (Dyersville)
- Missouri – Gateway Arch (St. Louis)
- North Dakota – Theodore Roosevelt National Park
- South Dakota – Mount Rushmore
- Nebraska – Chimney Rock
- Kansas – Monument Rocks
- Southwest
- Oklahoma – National Cowboy & Western Heritage - Museum
- Texas – The Alamo (San Antonio)
- New Mexico – Carlsbad Caverns
- Arizona – Grand Canyon National Park

#### West
- Colorado – Rocky Mountain National Park
- Wyoming – Yellowstone National Park / Old Faithful
- Montana – Glacier National Park
- Idaho – Shoshone Falls
- Utah – Arches National Park
- Nevada – Las Vegas Strip
- California – Golden Gate Bridge
- Oregon – Crater Lake National Park
- Washington – Space Needle (Seattle)
- Alaska – Denali (Mount McKinley)
- Hawaii – Pearl Harbor / USS Arizona Memorial