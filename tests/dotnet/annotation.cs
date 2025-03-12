namespace rhino3dm_test;

using Rhino.FileIO;
using Rhino.Geometry;

public class Annotation_Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Annotation_ReadFromFile()
    {

        var path = Directory.GetCurrentDirectory();

        //Console.WriteLine("Current Directory: {0}",path);

        File3dm file3dm = new File3dm();

        //the path will be different whether you are using dotnet run or debugging
        if(path.Contains("net")){
            file3dm = File3dm.Read("../../../../models/textEntities_r8.3dm");
        } else{
            file3dm = File3dm.Read("../models/textEntities_r8.3dm");
        } 

        Console.WriteLine("Number of objects in file {0}", file3dm.Objects.Count);

        string[] textArray = {"Hello World!", "Hello Cruel World!", "WTF"};

        foreach( var obj in file3dm.Objects){

            switch(obj.Geometry.ObjectType)
            {
                case Rhino.DocObjects.ObjectType.Annotation:
                    TextEntity textEntity = obj.Geometry as TextEntity;
                    if(!textArray.Contains(textEntity.PlainText))
                    {
                        Assert.Fail("TextEntity Plain Text not found in textArray");
                    }
                    break;
                
                case Rhino.DocObjects.ObjectType.TextDot:
                    TextDot textDot = obj.Geometry as TextDot;
                    if(!textArray.Contains(textDot.Text))
                    {
                        Assert.Fail("TextDot Text not found in textArray");
                    }
                    break;

            }
            

            
            
        }

        Assert.Pass();
    }
}