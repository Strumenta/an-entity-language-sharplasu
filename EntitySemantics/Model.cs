using System;
using System.Collections.Generic;
using System.Linq;
using Strumenta.Entity.Semantics;
using Strumenta.Sharplasu.Model;

namespace Strumenta.Entity
{
    public class Module : Node, Named
    {
        public string Name { get; set; }
        public List<Import> Imports { get; set; } = new List<Import>();
        public List<TypeDecl> Types { get; set; } = new List<TypeDecl>();
        public List<ClassDecl> Entities { get; set; } = new List<ClassDecl>();

        public Module(string name, List<Import> imports = null, List<TypeDecl>? types = null, List<ClassDecl>? entities = null)
        {
            Name = name;
            Imports = imports ?? new List<Import>();
            Types = types ?? new List<TypeDecl>();
            Entities = entities ?? new List<ClassDecl>();
        }
    }

    public class Import : Node
    {
        public ReferenceByName<Module> Module { get; set; }
        public Import(string name)
        {
            Module = new ReferenceByName<Module>(name);
        }
    }

    public class TypeDecl : Node, Named, IType
    {
        public string Name { get; set; }

        public TypeDecl(string name)
        {
            Name = name;
        }
    }

    public class ClassDecl : TypeDecl
    {
        public ReferenceByName<ClassDecl>? Superclass { get; set; }
        public List<FeatureDecl> Features { get; set; }

        public ClassDecl(string name, ReferenceByName<ClassDecl> superclass = null, List<FeatureDecl>? features = null)
            : base(name)
        {
            Name = name;
            Superclass = superclass;
            Features = features ?? new List<FeatureDecl>();
        }
    }

    public class FeatureDecl : Node, Named
    {
        public string Name { get; set; }
        public ReferenceByName<TypeDecl> Type { get; set; }
        public Expression? Value { get; set; }

        public FeatureDecl(string name, ReferenceByName<TypeDecl> type, Expression value = null) : base()
        {
            Name = name;
            Type = type;
            Value = value;
        }
    }

    public abstract class Expression : Node { }

    public enum Operator
    {
        Addition,
        Subtraction,
        Multiplication,
        Division
    }

    public class OperatorExpression : Expression
    {
        public Expression Left { get; set; }
        public Expression Right { get; set; }
        public Operator Operator { get; set; }

        public OperatorExpression(Expression left, Expression right, Operator operatorType)
        {
            Left = left;
            Right = right;
            Operator = operatorType;
        }
    }

    public class ReferenceExpression : Expression
    {
        public Expression? Context { get; set; }
        public ReferenceByName<FeatureDecl> Target { get; set; }
        public ReferenceExpression(Expression context, ReferenceByName<FeatureDecl> target)
        {
            Context = context;
            Target = target;
        }
    }

    public abstract class LiteralExpression : Expression { }

    public class StringLiteralExpression : LiteralExpression
    {
        public string Value { get; set; }

        public StringLiteralExpression(string value)
        {
            Value = value;
        }
    }

    public class BooleanLiteralExpression : LiteralExpression
    {
        public bool Value { get; set; }

        public BooleanLiteralExpression(bool value)
        {
            Value = value;
        }
    }

    public class IntegerLiteralExpression : LiteralExpression
    {
        public long Value { get; set; }

        public IntegerLiteralExpression(long value)
        {
            Value = value;
        }
    }

    public class RealLiteralExpression : LiteralExpression
    {
        public double Value { get; set; }

        public RealLiteralExpression(double value)
        {
            Value = value;
        }
    }
}
