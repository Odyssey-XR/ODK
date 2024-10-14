using System.Globalization;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace ODK.Kit.SourceGenerators
{
  [Generator]
  public class PartialMonoBehaviourSourceGenerator : ISourceGenerator
  {
    public void Initialize(GeneratorInitializationContext context)
    {
    }

    public void Execute(GeneratorExecutionContext context)
    {
      System.Console.WriteLine(System.DateTime.Now.ToString(CultureInfo.InvariantCulture));

      var sourceBuilder = new StringBuilder(
        @"
            using System;
            namespace ExampleSourceGenerated
            {
                public static class ExampleSourceGenerated
                {
                    public static string GetTestText()
                    {
                        return ""This is from source generator ");

      sourceBuilder.Append(System.DateTime.Now.ToString(CultureInfo.InvariantCulture));

      sourceBuilder.Append(
        @""";
                    }
    }
}
");

      context.AddSource("exampleSourceGenerator", SourceText.From(sourceBuilder.ToString(), Encoding.UTF8));
    }
  }
}