using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNET.Loader;
using PurchaseManagement.MVVM.ViewModels;
using ArchUnitNET.xUnit;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;
using System.ComponentModel;
using MVVM;

namespace PurchaseManagement.Test.Fitness
{
    public class NamingConventionTest
    {
        private static readonly Architecture Architecture = new ArchLoader().LoadAssemblies(System.Reflection.Assembly.Load("PurchaseManagement")).Build();

        [Fact]
        public void AllViewModel_ShouldEndWithViewModel()
        {
            //var rule = Interfaces()
            //    .That()
            //    .HaveName("BaseViewModel");
            //var x = rule.GetObjects(Architecture);
            Classes()
                 .That()
                 .AreAssignableTo(typeof(BaseViewModel))
                 .Should()
                 .HaveNameEndingWith("ViewModel")
                 .Check(Architecture);
        }
    }
}