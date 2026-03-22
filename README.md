This is a console application for managing equipment rentals at a university.
Users can rent laptops, cameras, projectors and return them. 
The system tracks all rentals including late returns and penalties.

To run the program you need to clone the repository and open the project in Rider or Visual Studio.
To run the program you shoul press the Run button.

project consists of: 
- 'Models' - all classes like User, Rental, Equipment
- 'Services' - RentalService with all  logic
- 'Enum' - Status types for users and equipment
- 'Interfaces' - IRentalService interface 
- 'Database' - Singleton class 

I decided to keep all logic in RentalService and not spread it across multiple classes.
Equipment class uses inheritance because Projector, Laptop, Camera all of them share common 
properties like status and name, but each of them has its own specific characteristics.
Rental limits for students and employees are defined in one place, easy to modify.

