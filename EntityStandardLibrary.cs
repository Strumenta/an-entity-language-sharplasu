using Strumenta.Entity;
using Strumenta.Sharplasu.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Strumenta.Entity
{
    public static class EntityStandardLibrary
    {
        public static Module StandardModule
        {
            get {
                var module = new Module(
                    name: "standard",                    
                    types: new List<TypeDecl>()
                    {
                        new TypeDecl("string"),
                        new TypeDecl("integer"),
                    }                    
                );
                module.AssignParents();
                return module;
            }
        }
    }
}
