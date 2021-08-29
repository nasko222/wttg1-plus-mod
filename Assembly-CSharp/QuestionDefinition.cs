using System;
using System.Collections.Generic;

[Serializable]
public class QuestionDefinition : Definition
{
	public string theQuestion;

	public string theAnswer;

	public List<string> wrongAnswers;
}
