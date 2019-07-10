using System;using System.Collections.Generic;
					
public static class Program
{
	  //add U+2019 from https://en.wikipedia.org/wiki/Apostrophe
    //you can add others, if you think so - http://www.fileformat.info/search/google.htm?q=+SINGLE+QUOTATION+MARK
		public static bool isUnicodeApostrophe(this char c)
		{
    		if (c == '\'' || c == '\u2019') return true;
            else return false; 

			//switch (c) {
			//	case '\'':
			//	case '\u2019': //’ Right Single Quotation Mark
			//		return true;
			//		break; //ignore error, this is quicker
			//	default:
			//		return false; 
			//		break; //ignore error, this is quicker
			//}

		}
        //TODO - add dictionary for 47,589 hyphenated words. Add parser for tokens and match! 
                 This infers another loop, but best optimially if you could do it in same loop below.
                 Get all 47,589 hyphenated words here - https://metadataconsulting.blogspot.com/2019/07/An-extensive-list-of-all-English-triple-Hyphenated-words.html

		    /// <summary>
        /// Counts words handling apostrophe in middle of a word and numbers. 
        /// Good for counting words of code.
        /// </summary>
        /// <param name="s">hidden, static</param>
        /// <returns></returns>
        public static int CountWordsforCode(this string s)
        {
            if (string.IsNullOrEmpty(s)) return 0; 
			int wc = 0;     //word count
            int apc = 0;    //apostrophe count
            int tc = 0;     //thousands count
            int dc = 0;     //decimal count or IP Address seperator, etc

            if (char.IsLetterOrDigit(s[0])) { wc = 1; } //bounds condition, is 1st char start of word?  
            			
            for (int i = 1; i < s.Length; i++)
            {
                //Edge Case  - apostrophe in middle of a word
                //FAILED = 1'1 - 2 words
                //PASS - "O'Connel" - 1 word
                //PASS - goodness' - 1 word //known formally as an Elision or broadly Contraction
                //PASS - 'em - 1 word //known formally as an Elision or more broadly Contraction
                if (i < s.Length - 1 && char.IsLetter(s[i - 1]) && s[i].isUnicodeApostrophe() && char.IsLetter(s[i + 1])) { 
                    apc++;
                    //Console.WriteLine(s + "     apostrophe cnt tc = " + apc);
                }
                //Edge Case - Numbers 1,000.00 -- thousands separator
                if (i < s.Length - 1 && char.IsNumber(s[i - 1]) && (s[i] == ',') && char.IsNumber(s[i + 1]))
                {
                    tc++;
                    //Console.WriteLine(s + "     thousands cnt tc = " + tc);
                }
                //Edge Case - Numbers 1,000.00 -- decimal
                if (i < s.Length - 1 && char.IsNumber(s[i - 1]) && (s[i] == '.') && char.IsNumber(s[i + 1]))
                {
                    dc++;
                    //Console.WriteLine(s + "     decimal cnt dc = " + dc);
                }
                
				//Main enumeration is pretty simple
                //detect previous character is word seperator aka boundary
                if (char.IsWhiteSpace(s[i - 1]) || char.IsPunctuation(s[i - 1]) || char.IsSymbol(s[i - 1]))
                {
                    if (char.IsLetterOrDigit(s[i])) //is current char start of a new word
                        wc++;
                }
				
            }
            //Console.WriteLine(s + "     end...  word c = " + wc);
            return wc - (apc * 2) + apc - (tc * 2) + tc - (dc * 2) + dc; 
        }
	    
	    public static void Main()
        {
			//CAN SOMEONE IMPLEMENT ALL THESE TEST CASES! TO MANY FOR ME RIGHT NOW
			//https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/
			Dictionary<string, bool> CSharpOperands = new Dictionary<string, bool>() 
			{
				//t ? x : y - super special edge case 
				//2 characters in len, true 
				{"<=",true},//binary op = space or letter or digit can surround this operator
				{">>=",true},//binary op
				{"<<=",true},//binary op
				{"/= ",true},//binary op
				{"||",true},//binary op
				{"|=",true},//binary op
				{"^=",true},//binary op
				{"?[",true},//binary op
				{"??",true},//binary op
				{"?.",true},//binary op
				{">>",true},//binary op
				{">=",true},//binary op
				{"==",true},//binary op
				{"<<",true},//binary op
				{"->",true},//binary op
				{"-=",true},//binary op
				{"--",true}, //--x prefix or suffix, not binary -> var x = 1 -- 2; is an error
				{"+=",true},//binary op
				{"++",true}, //++x prefix or suffix, not binary -> var x = 1 ++ 2; is an error
				{"*=",true},//binary op
				{"&=",true},//binary op
				{"&&",true},//binary op
				{"%=",true},//binary op
				{"=>",true},//binary op
				{"!=",true},//binary op 
				//1 character in leng, false 
				{"!",false}, //prefix operator
				{"~",false}, //prefix operator
				{"|",false}, //binary op only
				{".",false}, //binary op for words, not for numbers (decimal, considered 1 word 100.00)
				{"-",false}, //prefix operator & binary op
				{"+",false}, //prefix operator & binary op

			}; 
			
			//mainly edge cases
			string[] tokens = { "e", "ab", "abc", "abcdef", "", "a,b", "e e a e", "e}}}}}})*", 
							   "(CAN'T, DON'T)", "{'val1','val2'}", "\"what's here\"", 
							   "\"1'1 2 3 what's he’s isn't\"", "for goodness’ sake", 
							   "'em exuse me", "\"what 'dillygrout' is?\"", "\"word-for-word\"", 
							   "newToolStripMenuItem_Click(object sender, EventArgs e)", 
							   "(tabControl1.TabCount + 1).ToString();", 
							   "char.IsLetter(s[i-1])", 
							   "10.2c", "172.168.0.0", 
							   "1,000.00", "1,000.", 
							   "http://en.wikipedia.org/wiki/Rice's_theorem",
                               "<script type=\"text/javascript\">" };
            foreach (var token in tokens)
                Console.WriteLine(token + "\nword count = " + token.CountWordsforCode() + "\n");

             
        }

}
