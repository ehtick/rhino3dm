import rhino3dm
import unittest
import os.path
from os.path import dirname

class TestAnnotation(unittest.TestCase):
    def test_readAnnotations(self):

        model = rhino3dm.File3dm.Read("../models/textEntities_r8.3dm")

        if model is None:
            self.fail("Failed to read file")

        plainText = ["Hello World!", "Hello Cruel World!", "WTF"]
        for obj in model.Objects:
            geo = obj.Geometry

            if geo.ObjectType == rhino3dm.ObjectType.Annotation:
                if not any(x in geo.PlainText for x in plainText):
                        self.fail("Something wrong with Annotation.PlainText")
            elif geo.ObjectType == rhino3dm.ObjectType.TextDot:
                if not any(x in geo.Text for x in plainText):
                        self.fail("Something wrong with TextDot.Text")

if __name__ == '__main__':
    print("running tests")
    unittest.main()
    print("tests complete")