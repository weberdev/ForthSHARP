using System;
Stack<object> FStack = new Stack<object>();
List<Word> WordList = new List<Word> { };
string workingString = "";
int workingOn = 0;

startInterpreter();

void startInterpreter()
{
    Console.WriteLine("Welcome to forthSHARP. Please begin.");
    parseLine();
}

void handleToken(string tkn)
{
    if (workingOn == 1)
    {
        char lastChar = tkn[tkn.Length - 1];

        workingString = workingString + " " + tkn;
        if (lastChar == '"')
        {
            workingOn = 0;
            workingString = workingString.Substring(0, workingString.Length - 1);
            FStack.Push(workingString);
            workingString = "";
        }
    }
    else if (workingOn == 2)
    {
        if (tkn == ";")
        {
            
        }
    }
    else
    {
        double numHolder;
        if (WordList.Any(obj => obj.Name == tkn))
        {
         
            Word wordObj = WordList.FirstOrDefault(w => w.Name == tkn);

            // Check if the Word object exists and is a PreDefinedWord
            if (wordObj != null)
            {
                if (wordObj is PreDefinedWord)
                {
                    // Cast the Word object to a PreDefinedWord and execute its function
                    PreDefinedWord predefinedWord = (PreDefinedWord)wordObj;
                    predefinedWord.function();
                }
                else if(wordObj is UserDefinedWord)
                {
                    UserDefinedWord userDefinedWord = (UserDefinedWord)wordObj;
                    handleNewFunc(userDefinedWord.words);
                }
                else
                {
                    Console.Error.WriteLine("it's broken");
                }
            }
        }
        else if(double.TryParse(tkn, out numHolder)){
            FStack.Push(numHolder);
        }
        else if (isOperator(tkn))
        {
            stackMath(tkn);
        }
        else
        {
            Console.WriteLine("Invalid token.");
        }
    }
}

bool isOperator(string token)
{
    if (token == "+" || token == "-" || token == "*" || token == "/" || token == "mod")
    {
        return true;
    }
    else
    {
        return false;
    }
}

void parseLine(){
    string inputSTR = Console.ReadLine();
    if (inputSTR != null)
    {
        List<string> tokens = new List<string>();
        tokens = splitInput(inputSTR);
        foreach (string tkn in tokens)
        {
            handleToken(tkn);
        }
    }
        Console.WriteLine("ok");
    parseLine();
}

void handleNewFunc(string funcString)
{
    List<string> tokens = new List<string>();
    tokens = splitInput(funcString);
    foreach (string tkn in tokens)
    {
        handleToken(tkn);
    }
}

void stackMath(string op)
{
    if (FStack.Count < 2)
    {
        Console.WriteLine("insufficient stack");
        return;
    }
    string first = FStack.Pop().ToString();
    string second = FStack.Pop().ToString();
    double fst;
    double snd;
    double.TryParse(first, out fst);
    double.TryParse(second, out snd);
    double result;
    if (op == "+")
    {
        result = fst + snd;
    }
    else if (op == "-")
    {
        result=fst - snd;
    }
    else if (op == "*")
    { result = fst / snd;}
    else if (op == "/")
    {
        result = fst / snd;
    }
    else if (op == "mod")
    {
        result = fst % snd;
    }
    else
    {
        Console.Error.WriteLine("invalid op");
        return;
    }
    FStack.Push(result);
}

List<string> splitInput(string inputStr)
{
    List<string> tokens = new List<string>();
    tokens = inputStr.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
    return tokens;
}

public class Word
{
    public string Name;
}

public class PreDefinedWord : Word
{
    public Action function;
}

public class UserDefinedWord : Word
{
    public string words;
}

