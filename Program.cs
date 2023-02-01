// See https://aka.ms/new-console-template for more information
using RulesEngine.Extensions;
using RulesEngine.Models;
using System.Dynamic;

Console.WriteLine("Hello, World!");


List<Workflow> workflows = new List<Workflow>();
Workflow workflow = new Workflow();
workflow.WorkflowName = "Test Workflow Rule 1";

List<Rule> rules = new List<Rule>();

Rule rule = new Rule();
rule.RuleName = "Test Rule";
rule.SuccessEvent = "Count is within tolerance.";
rule.ErrorMessage = "Over expected.";
rule.Expression = "count < 0";
rule.RuleExpressionType = RuleExpressionType.LambdaExpression;



rules.Add(rule);

workflow.Rules = rules;

workflows.Add(workflow);


var bre = new RulesEngine.RulesEngine(workflows.ToArray());

dynamic datas = new ExpandoObject();
datas.count = 1;
var inputs = new dynamic[]
  {
        datas
  };

List<RuleResultTree> resultList =  await bre.ExecuteAllRulesAsync("Test Workflow Rule 1", inputs);

bool outcome = false;

//Different ways to show test results:
outcome = resultList.TrueForAll(r => r.IsSuccess);

resultList.OnSuccess((eventName) => {
    Console.WriteLine($"Result '{eventName}' is as expected.");
    outcome = true;
});

resultList.OnFail(() => {
    outcome = false;
});

Console.WriteLine($"Test outcome: {outcome}.");