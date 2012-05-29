KissDTO
=======

An extremely simple DTO mapper.

## Usage
It's as simple as

    var resultDto = someObject.As<SomeDto>();

The mapping is done based on the naming of the properties like

    public class Person
    {
        public long Id { get; set; }
        public string Firstname { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }

        public Location Residence { get; set; }
    }
    
could be mapped to

    public class PersonDto
    {
        public string Firstname { get; set; }
        public string Surname { get; set; }
    }
    
with the result of just having `Firstname` and `Surname` mapped.

## Credits
Original implementation taken from [imperugo.tostring.it](http://imperugo.tostring.it/blog/post/dto-il-e-reflection-nelle-nostre-applicazioni).