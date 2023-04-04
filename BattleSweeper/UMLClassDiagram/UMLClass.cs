using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using TextCopy;

namespace UMLClassDiagram
{

    public class UMLField : IComparable<UMLField>
    {
        public char access;
        public string name = "";
        public string type_name = "";

        public UMLField(char access, string name, string type_name)
        {
            this.access = access;
            this.name = name;
            this.type_name = type_name;
        }

        public UMLField(FieldInfo field_info)
        {
            if (field_info.IsPublic)
                access = '+';
            else if (field_info.IsFamily)
                access = '~';
            else if (field_info.IsPrivate)
                access = '-';

            name = field_info.Name;
            type_name = field_info.FieldType.Name;
        }

        public string generateString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append($"{access} {name}");

            if (type_name != "void")
                sb.Append($": {type_name}");

            return sb.ToString();
        }

        public int CompareTo(UMLField? other)
        {
            int access_this = -1;
            int access_other = -1;

            if (other.access == '+')
                access_other = 1;
            else if (other.access == '~')
                access_other = 2;
            else if (other.access == '-')
                access_other = 3;

            if (access == '+')
                access_this = 1;
            else if (access == '~')
                access_this = 2;
            else if (access == '-')
                access_this = 3;

            if (access_this != access_other)
                return access_this.CompareTo(access_other);

            return generateString().CompareTo(other.generateString());
        }
    }

    public class UMLMethod : IComparable<UMLMethod>
    {
        public char access;
        public string name = "";
        public string return_type = "";
        public List<string> input_types = new();

        public UMLMethod(char access, string name, string return_type, List<string> input_types)
        {
            this.access = access;
            this.name = name;
            this.return_type = return_type;
            this.input_types = input_types;
        }

        public UMLMethod(MethodInfo method_info)
        {
            if (method_info.IsPublic)
                access = '+';
            else if (method_info.IsFamily)
                access = '~';
            else if (method_info.IsPrivate)
                access = '-';

            name = method_info.Name;
            return_type = method_info.ReturnType.Name;

            foreach (var input_param in method_info.GetParameters())
                input_types.Add(input_param.ParameterType.Name ?? "");
        }

        public string generateString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append($"{access} {name}({string.Join(", ", input_types)})");

            if (return_type != "Void")
                sb.Append($": {return_type}");

            return sb.ToString();
        }

        public int CompareTo(UMLMethod? other)
        {

            int access_this = -1;
            int access_other = -1;

            if (other.access == '+')
                access_other = 1;
            else if (other.access == '~')
                access_other = 2;
            else if (other.access == '-')
                access_other = 3;

            if (access == '+')
                access_this = 1;
            else if (access == '~')
                access_this = 2;
            else if (access == '-')
                access_this = 3;

            if (access_this != access_other)
                return access_this.CompareTo(access_other);

            return generateString().CompareTo(other.generateString());
        }
    }

    public class UMLClass
    {

        public string class_name;
        
        public SortedSet<UMLField> fields = new();
        public SortedSet<UMLMethod> methods = new();

        public UMLClass(string class_name = "")
        {
            this.class_name = class_name;
        }

        public UMLClass(Type type_info)
        {
            class_name = type_info.Name;

            foreach (var member_field in type_info.GetFields(BINDINGS))
            {
                fields.Add(new(member_field));
            }

            foreach (var member_method in type_info.GetMethods(BINDINGS))
            {
                methods.Add(new(member_method));
            }

        }

        public string toDrawIOXML()
        {
            int item_height = 26;
            int item_count = 1;
            int id = 2;

            XNode root = new XElement("mxCell", new XAttribute("id", 1), new XAttribute("parent", 0));

            XElement top = new("mxGraphModel",
                new XElement("root",
                    new XElement("mxCell", new XAttribute("id", 0)),
                    root
                    ));


            // class UML box

            int extra_height = 0;

            if (fields.Count > 0 && methods.Count > 0)
                extra_height = 8;

            XElement tmp = new XElement("mxCell",
                    new XAttribute("id", id++),
                    new XAttribute("value", class_name),
                    new XAttribute("style", "swimlane;fontStyle=0;childLayout=stackLayout;startSize=26;horizontalStack=0;collapsible=1"),
                    new XAttribute("vertex", 1),
                    new XAttribute("parent", 1),
                new XElement("mxGeometry", new XAttribute("x", 140), new XAttribute("width", 160), new XAttribute("height", (fields.Count + methods.Count + 1) * item_height + extra_height), new XAttribute("as", "geometry")));
            root.AddAfterSelf(tmp);

            XElement curr = tmp;

            // field elements

            foreach(var field in fields)
            {
                tmp = new XElement("mxCell",
                    new XAttribute("id", id++),
                    new XAttribute("value", field.generateString()),
                    new XAttribute("style", "text;strokeColor=none;fillColor=none;align=left;verticalAlign=top;spacingLeft=4;spacingRight=4;overflow=hidden;rotatable=0;points=[[0,0.5],[1,0.5]];portConstraint=eastwest"),
                    new XAttribute("vertex", 1),
                    new XAttribute("parent", 2),
                    new XElement("mxGeometry", new XAttribute("y", item_count++ * item_height), new XAttribute("width", 160), new XAttribute("height", item_height), new XAttribute("as", "geometry")));
                curr.AddAfterSelf(
                    tmp
                    );
                curr = tmp;
            }

            // optional line

            if(fields.Count > 0 && methods.Count > 0)
            {
                tmp = new XElement("mxCell",
                    new XAttribute("id", id++),
                    new XAttribute("value", ""),
                    new XAttribute("style", "line;strokeWidth=1;align=left;verticalAlign=middle;spacingTop=-1;spacingLeft=3;spacingRight=3;points=[];portConstraint=eastwest;strokeColor=inherit"),
                    new XAttribute("vertex", 1),
                    new XAttribute("parent", 2),
                    new XElement("mxGeometry", new XAttribute("y", item_count * item_height + extra_height), new XAttribute("width", 160), new XAttribute("height", extra_height), new XAttribute("as", "geometry")));
                curr.AddAfterSelf(tmp);
                curr = tmp;

            }

            // method elements
            // field elements

            foreach (var method in methods)
            {
                tmp = new XElement("mxCell",
                    new XAttribute("id", id++),
                    new XAttribute("value", method.generateString()),
                    new XAttribute("style", "text;strokeColor=none;fillColor=none;align=left;verticalAlign=top;spacingLeft=4;spacingRight=4;overflow=hidden;rotatable=0;points=[[0,0.5],[1,0.5]];portConstraint=eastwest"),
                    new XAttribute("vertex", 1),
                    new XAttribute("parent", 2),
                    new XElement("mxGeometry", new XAttribute("y", item_count++ * item_height + extra_height), new XAttribute("width", 160), new XAttribute("height", item_height), new XAttribute("as", "geometry")));
                curr.AddAfterSelf(
                    tmp
                    );
                curr = tmp;
            }

            return top.ToString();
        }

        public void XMLToCB()
        {
            ClipboardService.SetText(toDrawIOXML());
        }

        protected readonly BindingFlags BINDINGS = BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly | BindingFlags.NonPublic | BindingFlags.Public;
    }
}
