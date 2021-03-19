using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace compilers
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            listview();
            textBox1.Visible = false;
            comboBox1.SelectedIndex = 0;
            //comboBox1.Visible = false;
            /*
             * 
             *  TreeNode node;
            
            node = treeFood.Nodes.Add("Fruits");
            node.Nodes.Add("Apple");
            node.Nodes.Add("Peach");
            
            node = treeFood.Nodes.Add("Vegetables");
            node.Nodes.Add("Tomato");
            node.Nodes.Add("Eggplant");
             * 
             * 
             * 
            TreeNode treeNode = new TreeNode("Windows");
            treeView1.Nodes.Add(treeNode);
            //
            // Another node following the first node.
            //
            treeNode = new TreeNode("Linux");
            treeView1.Nodes.Add(treeNode);
            //
            // Create two child nodes and put them in an array.
            // ... Add the third node, and specify these as its children.
            //
            TreeNode node2 = new TreeNode("C#");
            TreeNode node3 = new TreeNode("VB.NET");
            TreeNode[] array = new TreeNode[] { node2, node3 };
            //
            // Final node.
            //
            treeNode = new TreeNode("Dot Net Perls", array);
            treeView1.Nodes.Add(treeNode);
            
            */
        }
        string code;
        string tokens_code;
        string[] word = new string[1000];
        string[,] tokens = new string[1000,2];
        int no_tokens = 0;
        string[] num = new string[1000];
        int x = 0;
        int y = 0;
        int p = 0;
        private bool is_letter(char c)
        {
            if (c >= 'a' && c <= 'z' || c >= 'A' && c <= 'Z')
                return true;
            else return false;
        
        
        }
        private bool is_number(char c)
        {
            if (c >= '0'  && c <= '9' )
                return true;
            else return false;


        }

        private void listview()    //bta3t el listbox
        {
            // dataGridView1;
            listView1.Clear();
            listView1.View = View.Details;
            listView1.Columns.Add("token value", 200);
            listView1.Columns.Add("token type", 100);
           

            listView2.GridLines = true;
                    
            listView2.Clear();
            listView2.View = View.Details;
            listView2.Columns.Add("status", 200);
                                                  
            listView2.GridLines = true;
        }
        private void cout(string text)
        {
            listView2.Items.Add(text);
        }

        private void reset()
        {
            //Application.Restart();
             //richTextBox1.Clear();
             listview();
             button1.Enabled = true;
             button2.Enabled = true;
           
             Array.Clear(word,0,x+1);
             Array.Clear(tokens,0,x+1);
             Array.Clear(num, 0, x + 1);
             code = "";
             x = 0;
             y = 0;
             p = 0;
            
            tokens_code="";
            
            
            no_tokens = 0;
            treeView1.Nodes.Clear();

                      
            
        }

        private void print_tokens()
        {
            for (int i = 0; i < no_tokens; i++)
            {
                listView1.Items.Add(tokens[i,0]);
                listView1.Items[listView1.Items.Count - 1].SubItems.Add(tokens[i, 1]);

            
            }
            
        }

        private void scan_tokens()
        {
            string temp = "";
            for (int i = 0; i <tokens_code.Length;i++ ) 
           {
               if (tokens_code[i] == ' ') continue;
               else if (tokens_code[i] == ',') { tokens[no_tokens, 0] = temp; temp = "";  }
               else if (tokens_code[i] == '#' && temp != "") { tokens[no_tokens, 1] = temp; temp = ""; no_tokens++; }
               else if (tokens_code[i] == '#') { }
               else { temp += tokens_code[i]; }
            
            
            }
        
        }
        private void parse()
        {
            TreeNode node;
            node = treeView1.Nodes.Add("start");
            stmt_seq(node);
            treeView1.ExpandAll();
            if (no_tokens == 0) { MessageBox.Show("no tokens "); reset();  }
         

        }
        private bool match(string exp,string input)
        {
            if (exp == input) { p++; return true; }
            else { MessageBox.Show("Missing" + exp); reset(); return false; }

	    
	       
	       
        }

        private void statement(TreeNode node)
        {
            node = node.Nodes.Add("statement");
            if (tokens[p,0] == "if")
            {
                if_stmt(node);
            }
            else if (tokens[p,0] == "repeat")
            {
                repeat_stmt(node);
            }
            else if (tokens[p,0] == "read")
            {
                read_stmt(node);
            }
            else if (tokens[p,0] == "write")
            {
                write_stmt(node);
            }
            else if (tokens[p,1] == "IDENTIFIER")
            {
                assign_stmt(node);
            }
            else { node.Remove(); }
            //cout ("statement ok");
        }

        private void stmt_seq(TreeNode node)
        {
            //TreeNode node;
            //node = node.Nodes.Add("stmt_seq");
            statement(node);
            while (  tokens[p,0]==";")
            {
                match(";", tokens[p,0]);
                statement(node);
            }
            //cout ( "stmt-seq ok" );
        }

        private void if_stmt(TreeNode node)
        {
            //TreeNode node;
            node.Text="IF";
            match("if", tokens[p,0]);
            if (tokens[p, 0]=="(") match("(", tokens[p, 0]);
            exp(node);
            if (tokens[p, 0] == ")") match(")", tokens[p, 0]);
            match("then", tokens[p,0]);
            stmt_seq(node);
            if (tokens[p,0] == "else")
            {
                match("else", tokens[p,0]);
                stmt_seq(node);
            }
            match("END", tokens[p,1]);
            cout ( "if-stmt ok" );
        }

        //repeat-stmt -> repeat stmt-seq until exp
        private void repeat_stmt(TreeNode node)
        {
            //TreeNode node;
            node.Text = "REPEAT";
            match("repeat", tokens[p,0]);
            stmt_seq(node);
            match("until", tokens[p,0]);
            
            exp(node);
            cout ( "repeat-stmt ok" );
        }

        //read-stmt -> read IDENTIFIER
        private void read_stmt(TreeNode node)
        {
            //TreeNode node;
            //node.Text = "READ" + "(" + tokens[p, 0] + ")";
            match("read", tokens[p,0]);
            //node.Nodes.Add();
            node.Text = "READ" + "(" + tokens[p, 0] + ")";
            match("IDENTIFIER", tokens[p,1]);
            cout ( "read-stmt ok" );
        }

        //write-stmt -> write exp
        private void write_stmt(TreeNode node)
        {
            //TreeNode node;
            node.Text = "WRITE";
            match("write", tokens[p,0]);
            
            exp(node);
            cout ( "write-stmt ok" );
        }

        //assign-stmt -> IDENTIFIER := exp
        private void assign_stmt(TreeNode node)
        {
            //TreeNode node;
            node.Text = "ASSIGN";
            node.Text += "(" + tokens[p, 0] + ")";
            match("IDENTIFIER", tokens[p,1]);
            
            match("ASSIGN", tokens[p, 1]);
            
            exp(node);
            cout ( "assign-stmt ok" );
        }

        //exp -> simple-exp [comparison-op simple exp]
        private void exp(TreeNode node)
        {
            //TreeNode node;
            //node = treeView1.Nodes.Add(tokens[p, 1]);

            node = node.Nodes.Add(tokens[p, 1]);
            simple_exp(node);
            if (tokens[p,0] == "<" || tokens[p,0] == "=")
            {
                node.Text = tokens[p, 1];
                //node.Nodes.Add("(" + tokens[p, 0] + ")");
                comparison_op();
                simple_exp(node);
            }
            ////cout ( "exp" );
        }

        //simple-exp -> term {addop term}
        private void simple_exp(TreeNode node)
        {
            term(node);
            while (tokens[p,0] == "+" || tokens[p,0] == "-")
            {
                node.Text = tokens[p, 1];
                addop();
                term(node);
            }
            ////cout ("simple-exp");
        }

        //factor -> number | IDENTIFIER | ( exp )
        private void factor(TreeNode node)
        {
            if (tokens[p, 1] == "NUMBER") { node.Nodes.Add("(" + tokens[p, 0] + ")"); match("NUMBER", tokens[p, 1]); }
                ////cout ( "Number" );
           
            else if (tokens[p, 1] == "OPENBRACKET")
            {
                match(  "OPENBRACKET",tokens[p, 1]);
                exp(node);

                if (tokens[p, 1] =="CLOSEDBRACKET") {match("CLOSEDBRACKET", tokens[p, 1]); }
                else
                {

                    MessageBox.Show("Missing ')'");
                }
                ////cout ( "( exp )");	
            }
            else if ( tokens[p,1]=="IDENTIFIER")
            {
                node.Nodes.Add("(" + tokens[p, 0] + ")");
                match("IDENTIFIER", tokens[p, 1]);
                ////cout ( "IDENTIFIER" );	
            }
            else
            {
                cout ( "Expected number or IDENTIFIER or ( but found none." );
                MessageBox.Show("Expected number or IDENTIFIER or ( but found none.");
            }
            ////cout ( "factor" );
        }

        //term -> factor {mulop factor}
        private void term(TreeNode node)
        {
            factor(node);
            while (tokens[p,0] == "*" || tokens[p,0] == "/")
            {
                node.Text=tokens[p, 1];
                if (tokens[p, 0] == "*") match(tokens[p, 0], "*");
                if (tokens[p, 0] == "/") match(tokens[p, 0], "/");
                
                factor(node);
            }
            ////cout ("term");
        }

        //mulop -> * | /
        private void mulop()
        {
            if (tokens[p, 0] == "*") match(tokens[p, 0], "*");
            else if (tokens[p, 0] == "/") match(tokens[p, 0], "/");
            else
            {
                cout ( "Expected * or / but found none." );
            }
        }

        //addop -> + | -
        private void addop()
        {
            if (tokens[p,0] == "+")
            {
                match(tokens[p, 0], "+");
                ////cout ( "+" );
                //currToken = getNextToken(parserInput, curStt);
            }
            else if (tokens[p,0] == "-")
            {
                match("-",tokens[p, 0]) ;
                ////cout ( "-" );
                //currToken = getNextToken(parserInput, curStt);
            }
            else
            {
                cout ( "Expected + or - but found none." );
            }
        }

        //comparison-op -> < | =
        private void comparison_op()
        {   
            if (tokens[p,0] == "=")
            {
                ////cout ( "=" );
                //node.Nodes.Add("(" + tokens[p, 1] + ")");
                match(tokens[p, 0] , "=");
            }
            else if (tokens[p,0] == "<")
            {
                ////cout ( "<" );
                //node.Nodes.Add("(" + tokens[p, 1] + ")");
                match(tokens[p, 0], "<");
            }
            else
            {
                cout ( "Expected < or = but found none." );
            }
        }

        private void scan()
        {
            /*
             
             
             * de el function ely hatshta3'al feha 
             * kol kelma bete5azen fe array esmo word
             * 
             */



            string file_name = "output.txt";
           //string path= File.Create(file_name);
           StreamWriter sw = new StreamWriter(File.Create(file_name));
            
           // richTextBox1.Enabled = false;

           /* sw.Write("StartPositiwon");
            sw.Write(sw.NewLine);
            sw.Write("StartPositiwon");
            */
               // sw.Dispose();



            int i = 0;
           
            for (; i < code.Length; )//i++)

            {//law feh ay 7aga fel tiny de ana mesh katebha zawedha
                //if (code[i] == '\n' ) { i++; continue; }
                if (code[i] == ' ' ) { i++; continue; }
                if (code[i] == '+')//law feh idetifier na2sa kamlha || code[i] == '-' || code[i] == '*' || code[i] == '/' || code[i] == '<'

            {
                
                //listView1.Items.Add(Convert.ToString(code[i]));
                //listView1.Items[listView1.Items.Count - 1].SubItems.Add("PLUS");
                tokens[no_tokens,0]=code[i]+"";
                tokens[no_tokens, 1] = "PLUS";
                no_tokens++;
                sw.Write(sw.NewLine);
                sw.Write("\n" + Convert.ToString(code[i]) + ",PLUS"); i++; continue;
            }

                if (code[i] == '-')// code[i] == '*' || code[i] == '/' || code[i] == '<'
                {

                   
                    tokens[no_tokens, 0] = code[i] + "";
                    tokens[no_tokens, 1] = "MINUS";
                    no_tokens++;
                  
                    sw.Write(sw.NewLine);
                    sw.Write("\n" + Convert.ToString(code[i]) + ",MINUS"); i++; continue;
                }
                if (code[i] == '*')//  code[i] == '/' || code[i] == '<'
                {

                   
                    tokens[no_tokens, 0] = code[i] + "";
                    tokens[no_tokens, 1] = "MULT";
                    no_tokens++;
                    
                    sw.Write(sw.NewLine);
                    sw.Write("\n" + Convert.ToString(code[i]) + ",MULT"); i++; continue;
                }

                if (code[i] == '/')//  code[i] == '/' || code[i] == '<'
                {

                  
                    tokens[no_tokens, 0] = code[i] + "";
                    tokens[no_tokens, 1] = "DIV";
                    no_tokens++;
                  
                    sw.Write(sw.NewLine);
                    sw.Write("\n" + Convert.ToString(code[i]) + ",DIV"); i++; continue;
                }

                if (code[i] == '<')//  code[i] == '/' || code[i] == '<'
                {

                    tokens[no_tokens, 0] = code[i] + "";
                    tokens[no_tokens, 1] = "LESSTHAN";
                    no_tokens++;
                   
                    sw.Write(sw.NewLine);
                    sw.Write("\n" + Convert.ToString(code[i]) + ",LESSTHAN"); i++; continue;
                }

            if (code[i] == '=')
            {

               
                tokens[no_tokens, 0] = code[i] + "";
                tokens[no_tokens, 1] = "EQUAL";
                no_tokens++;
                
                sw.Write(sw.NewLine);
                sw.Write(Convert.ToString(code[i]) + ",EQUAL"); i++; continue;
            }
            if (code[i] == ';')
            {

               
                tokens[no_tokens, 0] = code[i] + "";
                tokens[no_tokens, 1] = "SEMICOLON";
                no_tokens++;
                
                sw.Write(sw.NewLine);
                sw.Write(Convert.ToString(code[i]) + ",SEMICOLON"); i++; continue;
            }
                if (code[i] == '(')
                {

                    
                    tokens[no_tokens, 0] = code[i] + "";
                    tokens[no_tokens, 1] = "OPENBRACKET";
                    no_tokens++;
                    
                    sw.Write(sw.NewLine);
                    sw.Write(Convert.ToString(code[i]) + ",OPENBRACKET"); i++; continue;
                }

                if (code[i] == ')')
                {

                    
                    tokens[no_tokens, 0] = code[i] + "";
                    tokens[no_tokens, 1] = "CLOSEDBRACKET";
                    no_tokens++;
                    
                    sw.Write(sw.NewLine);
                    sw.Write(Convert.ToString(code[i]) + ",CLOSEDBRACKET"); i++; continue;
                }
                if (code[i] == '{')//for comment
            {
                i++; if (i == code.Length) { MessageBox.Show("enter valid code please"); break; }
                    while (code[i]!='}')
                {
                    //word[x] += code[i];
                    i++; if (i == code.Length) { MessageBox.Show("enter valid code please"); break; }

                }
                i++;

                //listView1.Items.Add(word[x]);
                //listView1.Items[listView1.Items.Count - 1].SubItems.Add("comment");
                //sw.Write(sw.NewLine);
                //sw.Write(num[y] + ",comment");

            //    x++;
                continue;
                    //ehab
            }
            if (code[i] == ':')
            {
                if (code[i + 1] == '=')
                {
                    
                    tokens[no_tokens, 0] = ":=";
                    tokens[no_tokens, 1] = "ASSIGN";
                    no_tokens++;
                    
                    sw.Write(sw.NewLine);
                    sw.Write(":=" + ",ASSIGN");
                    i+=2; continue;
                }
            }

            if (is_letter(code[i]))
            {
                while (is_letter(code[i]) || is_number(code[i]) )
                {
                    if (is_number(code[i]))
                    {
                        MessageBox.Show("enter valid code please");
                        reset(); break; 
                    }
                    word[x] += code[i];
                    i++; if (i == code.Length) break;
                    
                }

                //e3ml hena el error check law feh IDENTIFIER feh 7aga 3'lt aw kdas

                if (word[x] == "if"||word[x] == "then"||word[x] == "else"||word[x] == "end"||word[x] == "repeat"||word[x] == "until"||word[x] == "read"||word[x] == "write")
                {
                   
                    tokens[no_tokens, 0] = word[x] ;
                    tokens[no_tokens, 1] = word[x].ToUpper();
                    no_tokens++;
                    
                    sw.Write(sw.NewLine);
                    sw.Write(word[x] + ","  +word[x].ToUpper() );
                }
               /* else if (word[x] == "IF" || word[x] == "THEN" || word[x] == "ELSE" || word[x] == "END" || word[x] == "REPEAT" || word[x] == "UNTIL" || word[x] == "READ" || word[x] == "WRITE")
                {
                    listView1.Items.Add(word[x]);
                    listView1.Items[listView1.Items.Count - 1].SubItems.Add(word[x]);
                    sw.Write(sw.NewLine);
                    sw.Write(word[x] + "," + word[x] );
                }*/
                else
                {
                    tokens[no_tokens, 0] = word[x];
                    tokens[no_tokens, 1] = "IDENTIFIER";
                    no_tokens++;
                   
                    sw.Write(sw.NewLine);
                    sw.Write(word[x] + ",IDENTIFIER");
                }
                x++;
                continue;
            }
             if (is_number(code[i]))
            {


                while (is_letter(code[i]) || is_number(code[i]))
                {
                    if (is_letter(code[i]))
                    {
                        MessageBox.Show("enter valid code please");
                        reset(); break;
                    }
                    num[y] += code[i];
                    i++; if (i == code.Length) break;

                }
                tokens[no_tokens, 0] = num[y];
                tokens[no_tokens, 1] = "NUMBER";
                no_tokens++;
                
                sw.Write(sw.NewLine);
                sw.Write(num[y] + ",NUMBER");
               
                y++;
                continue;

            }

             //i++;
             MessageBox.Show("enter valid code please");
             reset();
             
            }

           sw.Dispose();
           
        }


        private void button1_Click(object sender, EventArgs e)
        {
            //if (textBox1.Text == "" || textBox1.Text == " ") { MessageBox.Show("empty input"); reset(); }
            

            if (comboBox1.SelectedItem.ToString() == "code")
            {
                code = richTextBox1.Text;
                code = code.Replace("\n", " ");
                code = code.Replace("\r", " ");
                code = code.Replace("\t", " ");
                scan();
                print_tokens();
                parse();
            }
            else if (comboBox1.SelectedItem.ToString() == "tokens list")
            {
                tokens_code = richTextBox1.Text;
                tokens_code = tokens_code.Replace("\n", "#");
                tokens_code = tokens_code.Replace("\r", " ");
                tokens_code = tokens_code.Replace("\t", " ");
                tokens_code += '#';
                scan_tokens();
                print_tokens();
                parse();
                
            }
            button1.Enabled = false;
            button2.Enabled = false;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;
            //button1.Enabled = false;
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                InitialDirectory = @"D:\",
                Title = "Browse Text Files",

                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = "txt",
                Filter = "txt files (*.txt)|*.txt",
                FilterIndex = 2,
                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = true
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (textBox1.Text != "")
                    textBox1.Text = openFileDialog1.FileName;

                code = File.ReadAllText(openFileDialog1.FileName);
                richTextBox1.Text = code;
            } 
          

        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Application.Restart();    
            reset();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (comboBox1.SelectedItem.ToString() == "scanner") { button1.Text = "scan"; }
           // if (comboBox1.SelectedItem.ToString() == "parser") { button1.Text = "parse"; }
           // MessageBox.Show(comboBox1.SelectedItem.ToString());

        }
    }
}
