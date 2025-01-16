using Strumenta.Entity;
using Strumenta.Entity.Semantics;
using Strumenta.Sharplasu.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Strumenta.Entity
{
    public static class EntityStandardLibrary
    {
        public static TypeDecl StringType = new TypeDecl("string");
        public static TypeDecl IntegerType = new TypeDecl("integer");
        public static TypeDecl BooleanType = new TypeDecl("boolean");

        public static Module StandardModule
        {
            get {
                var module = new Module(
                    name: "standard",                    
                    types: new List<TypeDecl>()
                    {
                        StringType,
                        IntegerType,
                        BooleanType
                    }
                );
                module.AssignParents();
                return module;
            }
        }
    }
}
