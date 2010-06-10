using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Web.Configuration;
using System.IO;
using System.Net.Mime;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;

namespace UtilityExtensions
{
    public static partial class Util
    {
        private enum State
        {
            AtBeginningOfToken,
            InNonQuotedToken,
            InQuotedToken,
            ExpectingComma,
            InEscapedCharacter
        };

        public static string[] SplitCSV(this string source)
        {
            var splitString = new List<string>();
            List<int> slashesToRemove = null;
            var state = State.AtBeginningOfToken;
            var sourceCharArray = source.ToCharArray();
            var tokenStart = 0;
            var len = sourceCharArray.Length;
            for (int i = 0; i < len; ++i)
                switch (state)
                {
                    case State.AtBeginningOfToken:
                        if (sourceCharArray[i] == '"')
                        {
                            state = State.InQuotedToken;
                            slashesToRemove = new List<int>();
                            continue;
                        }
                        if (sourceCharArray[i] == ',')
                        {
                            splitString.Add("");
                            tokenStart = i + 1;
                            continue;
                        }
                        state = State.InNonQuotedToken;
                        continue;
                    case State.InNonQuotedToken:
                        if (sourceCharArray[i] == ',')
                        {
                            splitString.Add(
                                source.Substring(tokenStart, i - tokenStart));
                            state = State.AtBeginningOfToken;
                            tokenStart = i + 1;
                        }
                        continue;
                    case State.InQuotedToken:
                        if (sourceCharArray[i] == '"')
                        {
                            state = State.ExpectingComma;
                            continue;
                        }
                        if (sourceCharArray[i] == '\\')
                        {
                            state = State.InEscapedCharacter;
                            slashesToRemove.Add(i - tokenStart);
                            continue;
                        }
                        continue;
                    case State.ExpectingComma:
                        if (sourceCharArray[i] != ',')
                            throw new Exception("Expecting comma");
                        string stringWithSlashes =
                            source.Substring(tokenStart, i - tokenStart);
                        foreach (int item in slashesToRemove.Reverse<int>())
                            stringWithSlashes =
                                stringWithSlashes.Remove(item, 1);
                        splitString.Add(
                            stringWithSlashes.Substring(1,
                                stringWithSlashes.Length - 2));
                        state = State.AtBeginningOfToken;
                        tokenStart = i + 1;
                        continue;
                    case State.InEscapedCharacter:
                        state = State.InQuotedToken;
                        continue;
                }
            switch (state)
            {
                case State.AtBeginningOfToken:
                    splitString.Add("");
                    return splitString.ToArray();
                case State.InNonQuotedToken:
                    splitString.Add(
                        source.Substring(tokenStart,
                            source.Length - tokenStart));
                    return splitString.ToArray();
                case State.InQuotedToken:
                    throw new Exception("Expecting ending quote");
                case State.ExpectingComma:
                    string stringWithSlashes =
                        source.Substring(tokenStart, source.Length - tokenStart);
                    foreach (int item in slashesToRemove.Reverse<int>())
                        stringWithSlashes = stringWithSlashes.Remove(item, 1);
                    splitString.Add(
                        stringWithSlashes.Substring(1,
                            stringWithSlashes.Length - 2));
                    return splitString.ToArray();
                case State.InEscapedCharacter:
                    throw new Exception("Expecting escaped character");
            }
            throw new Exception("Unexpected error");
        }
    }
}
