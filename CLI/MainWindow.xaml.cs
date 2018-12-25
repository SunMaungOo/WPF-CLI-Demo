using CLI.Command;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CLI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private List<Command.ICommand> commandList;

        private TextBoxWriter writer;

        private bool commandMatch;


        public MainWindow()
        {
            InitializeComponent();

            InitCommands();

            commandMatch = false;
        }

        private void InitCommands()
        {
            HelpCommand helpCommand = new HelpCommand();

            commandList = new List<Command.ICommand>();
            commandList.Add(new WriteCommand());
            commandList.Add(new ClearCommand());
            commandList.Add(new ExitCommand(this));
            commandList.Add(helpCommand);


            helpCommand.Set(commandList.ToArray());

            writer = new TextBoxWriter(txtDisplay);
        }

        private void txtCommand_KeyUp(object sender, KeyEventArgs e)
        {
            switch(e.Key)
            {
                case Key.Enter:

                    if(lstAutoSuggest.Visibility==Visibility.Visible)
                    {
                        //if item is selected 

                        if(lstAutoSuggest.SelectedIndex>=0)
                        {
                            this.txtCommand.Text = lstAutoSuggest.SelectedItem.ToString();

                            this.txtCommand.CaretIndex = this.txtCommand.Text.Length;

                        }

                        lstAutoSuggest.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        ProcessCommand(txtCommand.Text);

                        txtCommand.Text = string.Empty;
                    }

                    break;

                case Key.OemPeriod:

                    if (PopulateAutoSuggestion())
                    {
                        if (lstAutoSuggest.Visibility != Visibility.Visible)
                        {
                            lstAutoSuggest.Visibility = Visibility.Visible;
                        }
                    }

                    break;

                case Key.Down:

                    if(lstAutoSuggest.Visibility==Visibility.Visible)
                    {
                        if(lstAutoSuggest.SelectedIndex<lstAutoSuggest.Items.Count-1)
                        {
                            lstAutoSuggest.SelectedIndex = lstAutoSuggest.SelectedIndex + 1;
                        }
                    }

                    break;

                case Key.Up:

                    if(lstAutoSuggest.Visibility==Visibility.Visible)
                    {
                        if(lstAutoSuggest.SelectedIndex>0)
                        {
                            lstAutoSuggest.SelectedIndex = lstAutoSuggest.SelectedIndex - 1;
                        }
                    }

                    break;

            }

            
        }

        private void ProcessCommand(string commandText)
        {
            if (commandText.Length > 0)
            {
                string[] tmp = commandText.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                string commandName = tmp[0];

                Command.ICommand command =SearchCommand(commandName);

                if (command != null)
                {
                    CommandContext context = new CommandContext();
                    context.StdError = writer;
                    context.StdOut = writer;

                    if (tmp.Length >= 2)
                    {
                        string[] args = new string[tmp.Length - 1];


                        for(int i=1,index=0;i<tmp.Length;i++,index++)
                        {
                            args[index] = tmp[i];
                        }

                        command.Run(args, context);
                    }
                    else
                    {
                        command.Run(new string[] { }, context);

                    }
                }
            }
        }

        /// <summary>
        /// Return ICommand or the null if the command is not found
        /// </summary>
        /// <param name="commandName">command name to search</param>
        /// <returns></returns>
        private Command.ICommand SearchCommand(string commandName)
        {
            foreach(Command.ICommand command in commandList)
            {
                if(command.Name==commandName)
                {
                    return command;
                }
            }


            return null;

        }

        private void lstAutoSuggest_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //Item double clicked,select it
            if(this.lstAutoSuggest.SelectedItems.Count==1)
            {
                this.commandMatch = true;
                this.SelectItem();
                this.commandMatch = false;
                this.txtCommand.Focus();
            }
        }

        private void lstAutoSuggest_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Make sure when the item is selected, focus is return to command text box

            this.txtCommand.Focus();
        }

        private void SelectItem()
        {
            if(commandMatch)
            {
                int selectionStart = this.txtCommand.SelectionStart;
                int prefixend = this.txtCommand.SelectionStart - txtCommand.Text.Length;
                int suffixstart = this.txtCommand.SelectionStart + txtCommand.Text.Length;

                if (suffixstart >= this.txtCommand.Text.Length)
                {
                    suffixstart = this.txtCommand.Text.Length;
                }

                string prefix = this.txtCommand.Text.Substring(0, prefixend);
                string fill = this.lstAutoSuggest.SelectedItem.ToString();
                string suffix = this.txtCommand.Text.Substring(suffixstart, this.txtCommand.Text.Length - suffixstart);

                this.txtCommand.Text = prefix + fill + suffix;
                this.txtCommand.SelectionStart = prefix.Length + fill.Length;

                lstAutoSuggest.Items.Clear();
                lstAutoSuggest.Visibility = Visibility.Collapsed;
            }
        }

    
        /// <summary>
        /// Populate the AutoSuggest box.
        /// Return true if atleast one command name match
        /// Return false if there is no match
        /// </summary>
        /// <returns></returns>
        private bool PopulateAutoSuggestion()
        {
            lstAutoSuggest.Items.Clear();

            bool isAutoSuggestPopulated = false;

            string word = GetTypedCommand();


            foreach (Command.ICommand command in commandList)
            {

                //remove all the command name which is less than the word we have type to save time

                if (command.Name.Length >= word.Length)
                {
                    bool isMatch = true;

                    // loop through all the type word

                    for (int i = 0; i < word.Length; i++)
                    {
                        //if the word previously type is the same as the part of command name (previous typed)
                        // for example the word is "wri" and the command name is "write"
                        // it check if the "wri" is found at the start of the command name

                        if (word[i] != command.Name[i])
                        {
                            //if the word does not match, game over. It is not this command

                            isMatch = false;

                            break;
                        }
                    }

                    if (isMatch)
                    {
                        isAutoSuggestPopulated = true;

                        lstAutoSuggest.Items.Add(command.Name);
                    }
                }

            }
            

            commandMatch = isAutoSuggestPopulated;

            return isAutoSuggestPopulated;
        }

        private string GetTypedCommand()
        {
            string typedCommand = string.Empty;

            //ignore ".",that why greater than 1

            if(txtCommand.Text.Length==1)
            {
                typedCommand = string.Empty;
            }
            else if(txtCommand.Text.Length>1)
            {
                //-1 since we don't want to have dot

                typedCommand = txtCommand.Text.Substring(0, txtCommand.Text.Length - 1);
            }

            return typedCommand;
        }

       
    }


    public class TextBoxWriter : TextWriter
    {
        private TextBlock txtBlock;

        private StringBuilder builder;

        public TextBoxWriter(TextBlock txtBlock)
        {
            this.txtBlock = txtBlock;

            this.builder = new StringBuilder();

        }

        public override Encoding Encoding
        {
            get
            {
                return new ASCIIEncoding();
            }
        }

        public override void WriteLine(string value)
        {

            this.builder.AppendLine(value);

            this.txtBlock.Text = builder.ToString();

        }

        public override void Flush()
        {
            this.builder.Clear();

            this.txtBlock.Text = builder.ToString();
        }


    }
}
