using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Data;
using System.Windows.Media;
using System.Text;

namespace EducationalProgram_PaskalProcAndFunc
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int _indexOfCurrentTheoryChapter = 0;
        private int _indexOfCurrentTest = 0;
        private List<TestQuestionWithAnswerModel> _selectedTests;
        private List<int> _userAnswers;
        private List<CheckBox> _currentQuestionAnswers;

        private readonly TestQuestionAnswerRepository _testQuestionAnswerRepository;
        private readonly TheoryRepository _theoryRepository;
        private readonly DocumentLoader _documentLoader;

        private const int AmountOfTestQuestions = 10;
        private const int AmountOfPossibleAnswers = 5;
        private const int NotSelectedAnswerValue = -1;

        public MainWindow()
        {
            _testQuestionAnswerRepository = new TestQuestionAnswerRepository();
            _theoryRepository = new TheoryRepository();
            _documentLoader = new DocumentLoader();

            InitializeComponent();

            treeViewContent.ItemsSource = _theoryRepository.TheoryChapterPathes.Value.Select(p => new TreeViewItem { Header = p.Item1 } );
            LoadAndShowTheoryChapter(_indexOfCurrentTheoryChapter);
        }

        private void menuitemTheory_Click(object sender, RoutedEventArgs e)
        {
            ChangeContentVisibilility(ContentType.Theory);
        }
        private void menuitemTests_Click(object sender, RoutedEventArgs e)
        {
            InitializeTests();

            ChangeContentVisibilility(ContentType.Tests);
        }
        private void menuitemAbout_Click(object sender, RoutedEventArgs e)
        {
            LoadAboutContent();

            ChangeContentVisibilility(ContentType.About);
        }

        private void ChangeContentVisibilility(ContentType contentType)
        {
            switch (contentType)
            {
                case ContentType.Theory:
                    {
                        HideAllContent();
                        this.GridTheory.Visibility = Visibility.Visible;
                    }
                    break;
                case ContentType.Tests:
                    {
                        HideAllContent();
                        this.GridTests.Visibility = Visibility.Visible;
                    }
                    break;
                case ContentType.About:
                    {
                        HideAllContent();
                        this.GridAbout.Visibility = Visibility.Visible;
                    }
                    break;
                default:
                    break;
            }
        }
        private void HideAllContent()
        {
            this.GridTheory.Visibility = Visibility.Collapsed;
            this.GridTests.Visibility = Visibility.Collapsed;
            this.GridAbout.Visibility = Visibility.Collapsed;
        }


        private void LoadAndShowTheoryChapter(int indexOfChapter)
        {
            string path = _theoryRepository.TheoryChapterPathes.Value[indexOfChapter].Item2;
            richTextBoxTheory.Document = _documentLoader.LoadDocument(path);
        }

        private void treeViewContent_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if(treeViewContent.SelectedItem != null)
            {
                _indexOfCurrentTheoryChapter = treeViewContent.Items.IndexOf(treeViewContent.SelectedItem);
            }
            
            LoadAndShowTheoryChapter(_indexOfCurrentTheoryChapter);
            UpdateNavigationButtonsVisibility();
        }

        private void TheoryPrevious_Click(object sender, RoutedEventArgs e)
        {
            if (_indexOfCurrentTheoryChapter > 0)
            {
                _indexOfCurrentTheoryChapter--;
                ((TreeViewItem)treeViewContent.Items[_indexOfCurrentTheoryChapter]).IsSelected = true;
            }
        }

        private void TheoryNext_Click(object sender, RoutedEventArgs e)
        {
            if (_indexOfCurrentTheoryChapter < treeViewContent.Items.Count - 1)
            {
                _indexOfCurrentTheoryChapter++;
                ((TreeViewItem)treeViewContent.Items[_indexOfCurrentTheoryChapter]).IsSelected = true;
            }
        }

        private void UpdateNavigationButtonsVisibility()
        {
            theoryPreviousBtn.IsEnabled = _indexOfCurrentTheoryChapter != 0;
            theoryNextBtn.IsEnabled = _indexOfCurrentTheoryChapter != treeViewContent.Items.Count - 1;
        }

        #region Tests

        private void InitializeTests()
        {
            _indexOfCurrentTest = 0;
            _selectedTests = _testQuestionAnswerRepository.Questions.Value.Shuffle().Take(AmountOfTestQuestions).ToList();
            treeViewTests.ItemsSource = _selectedTests.Select((p, index) => new TreeViewItem { Header = string.Format("Питання #{0}", index + 1) });
            labelQuestionsLeft.Content = GetQuestionsLeftText(treeViewTests.Items.Count);

            foreach (var item in _selectedTests)
            {
                item.Answers = item.Answers.Where(p => p.Second)
                    .Concat(item.Answers.Where(p => !p.Second).ToList().Shuffle().Take(AmountOfPossibleAnswers - item.Answers.Count(p => p.Second))).ToList().Shuffle();
            }
            
            _userAnswers = new List<int>();
            for (int i = 0; i < treeViewTests.Items.Count; i++)
            {
                _userAnswers.Add(NotSelectedAnswerValue);
            }

            _currentQuestionAnswers = new List<CheckBox>();
            foreach (var item in gridAnswers.Children)
            {
                if (item is CheckBox)
                {
                    _currentQuestionAnswers.Add(item as CheckBox);
                }
            }

            ShowTest(_indexOfCurrentTest);
        }

        private string GetQuestionsLeftText(int amountOfQuestions)
        {
            return new StringBuilder(string.Format("Залишилося {0} ", amountOfQuestions))
                .Append(amountOfQuestions > 4 || amountOfQuestions == 0 ? "питань" : "питання")
                .ToString();
        }

        private void ShowTest(int indexOfTest)
        {
            TestQuestionWithAnswerModel currentTest = _selectedTests[indexOfTest];
            textBoxQuestion.Text = currentTest.Question;

            for (int i = 0; i < currentTest.Answers.Count; i++)
            {
                _currentQuestionAnswers[i].GetChildOfType<TextBlock>().Text = currentTest.Answers[i].First;
            }

            _currentQuestionAnswers.ForEach(p => p.IsChecked = false);
            if (_userAnswers[indexOfTest] != NotSelectedAnswerValue)
            {
                _currentQuestionAnswers[_userAnswers[indexOfTest]].IsChecked = true;
            }
        }

        private void checkBox_Checked(object sender, RoutedEventArgs e)
        {
            foreach (var item in _currentQuestionAnswers)
            {
                if (item != sender as CheckBox)
                {
                    item.IsChecked = false;
                }
            }
        }
        private void buttonConfirm_Click(object sender, RoutedEventArgs e)
        {
            if(_currentQuestionAnswers.Any(p => p.IsChecked == true))
            {
                _userAnswers[_indexOfCurrentTest] = _currentQuestionAnswers.IndexOf(_currentQuestionAnswers.Find(p => p.IsChecked == true));
                (treeViewTests.Items[_indexOfCurrentTest] as TreeViewItem).Background = Brushes.LimeGreen;
            }
            else
            {
                _userAnswers[_indexOfCurrentTest] = NotSelectedAnswerValue;
                (treeViewTests.Items[_indexOfCurrentTest] as TreeViewItem).Background = Brushes.Yellow;
            }

            labelQuestionsLeft.Content = GetQuestionsLeftText(treeViewTests.Items.Count - _userAnswers.Count(p => p != NotSelectedAnswerValue));

            if (_indexOfCurrentTest == treeViewTests.Items.Count - 1)
            {
                buttonFinishTests_Click(sender, e);
            }
            else
            {
                testsNextBtn_Click(sender, e);
            }
        }

        private void buttonFinishTests_Click(object sender, RoutedEventArgs e)
        {
            string dialogMessage = _userAnswers.Any(p => p == NotSelectedAnswerValue) ? 
                "Є питання без підповіді, Ви дійсно бажаєте завершити тестування?" :
                "Ви дійсно бажаєте завершити тестування?";
            MessageBoxResult dialogResult = MessageBox.Show(dialogMessage, "Завершити тестування?", MessageBoxButton.YesNo);
            switch (dialogResult)
            {
                case MessageBoxResult.Yes:
                    {
                        int mark = 0;
                        for (int i = 0; i < _selectedTests.Count; i++)
                        {
                            if (_selectedTests[i].Answers.IndexOf(_selectedTests[i].Answers.Find(p => p.Second == true)) == _userAnswers[i])
                            {
                                mark++;
                            }
                        }

                        MessageBox.Show(string.Format("Ваш результат {0} з {1}", mark, treeViewTests.Items.Count));
                        menuitemTheory_Click(sender, e);
                    }
                    break;
                default:
                    break;
            }
        }

        private void treeViewTests_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (treeViewTests.SelectedItem != null)
            {
                _indexOfCurrentTest = treeViewTests.Items.IndexOf(treeViewTests.SelectedItem);
            }

            ShowTest(_indexOfCurrentTest);
            UpdateNavigationForTests();
        }
        private void testsNextBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_indexOfCurrentTest < treeViewTests.Items.Count - 1)
            {
                _indexOfCurrentTest++;
                ((TreeViewItem)treeViewTests.Items[_indexOfCurrentTest]).IsSelected = true;
            }
        }
        private void testsPreviousBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_indexOfCurrentTest > 0)
            {
                _indexOfCurrentTest--;
                ((TreeViewItem)treeViewTests.Items[_indexOfCurrentTest]).IsSelected = true;
            }
        }

        private void UpdateNavigationForTests()
        {
            labelQuestion.Content = string.Format("Питання #{0}", _indexOfCurrentTest + 1);
            testsPreviousBtn.IsEnabled = _indexOfCurrentTest != 0;
            testsNextBtn.IsEnabled = _indexOfCurrentTest != treeViewTests.Items.Count - 1;
        }

        #endregion

        private void LoadAboutContent()
        {
            richTextBoxAbout.Document = _documentLoader.LoadDocument(@"../../Resources/About.rtf");
        }
    }
}
