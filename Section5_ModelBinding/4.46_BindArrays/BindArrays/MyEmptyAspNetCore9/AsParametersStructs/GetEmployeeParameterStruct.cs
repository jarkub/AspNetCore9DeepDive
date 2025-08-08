using Microsoft.AspNetCore.Mvc;

namespace UseAsParameters.AsParametersStructs;

// Can be class or struct
struct GetEmployeeParameterStruct
{
    [FromRoute]
    public int Id { get; set; }

    [FromQuery]
    public string Name { get; set; }

    [FromHeader]
    public string Position { get; set; }

    public GetEmployeeParameterStruct(int id, string name, string position)
    {
        Id = id;
        Name = name;
        Position = position;
    }
}

class GetEmployeeParameterClass
{
    [FromRoute]
    public int Id { get; set; }

    [FromQuery]
    public string Name { get; set; }

    [FromHeader]
    public string Position { get; set; }

    public GetEmployeeParameterClass(int id, string name, string position)
    {
        Id = id;
        Name = name;
        Position = position;
    }
}