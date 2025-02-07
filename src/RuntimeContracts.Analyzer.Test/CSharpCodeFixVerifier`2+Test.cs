﻿using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Testing.Verifiers;

namespace RuntimeContracts.Analyzer.Test
{
    public static partial class CSharpCodeFixVerifier<TAnalyzer, TCodeFix>
        where TAnalyzer : DiagnosticAnalyzer, new()
        where TCodeFix : CodeFixProvider, new()
    {
        public class Test : CSharpCodeFixTest<TAnalyzer, TCodeFix, MSTestVerifier>
        {
            public Test()
            {
                SolutionTransforms.Add((solution, projectId) =>
                {
                    var project = solution.GetProject(projectId);
                    var parseOptions = (CSharpParseOptions)project.ParseOptions;
                    solution = solution.WithProjectParseOptions(projectId, parseOptions.WithLanguageVersion(LanguageVersion));

                    if (IncludeContracts)
                    {
                        solution = solution.AddMetadataReference(projectId, AdditionalMetadataReferences.RuntimeContracts)
                            .AddMetadataReference(projectId, AdditionalMetadataReferences.SystemRuntime);
                    }

                    return solution;
                });
            }

            public bool IncludeContracts { get; set; } = true;

            public LanguageVersion LanguageVersion { get; set; } = LanguageVersion.CSharp7_1;
        }
    }
}
