using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrelloApps
{
    public partial class TrelloManagement : Form
    {
        public TrelloManagement()
        {
            InitializeComponent();
        }
        public class TrelloCard
        {
            public TrelloCard(string title)
            {
                Title = title;
            }

            public string Title { get; set; }

        }
        public class TrelloList
        {
            public string Name { get; set; }
            public LinkedList<TrelloCard> Cards { get; set; }

            public TrelloList(string name)
            {
                Name = name;
                Cards = new LinkedList<TrelloCard>();
            }
        }
        public class TrelloBoard
        {
            public string Name { get; set; }
            public LinkedList<TrelloList> Lists { get; set; }
            

            public TrelloBoard(string name)
            {
                Name = name;
               Lists = new LinkedList<TrelloList>();
            }

            public void AddList(string listName)
            {

                var newList = new TrelloList(listName);
                Lists.AddLast(newList);
            }

            public void AddCardToList(TrelloList list, string title)
            {
                var newCard = new TrelloCard(title);
                list.Cards.AddLast(newCard);
            }

            internal void AddCardToList(TrelloList selectedList)
            {
                throw new NotImplementedException();
            }
        }
        private LinkedList<TrelloBoard> ListBoards = new LinkedList<TrelloBoard>();
        private LinkedList<TrelloList> CurrentLists = new LinkedList<TrelloList>();
        private LinkedList<TrelloCard> CurrentCards = new LinkedList<TrelloCard>();
        private void ListBoard_SelectedIndexChanged(object sender, EventArgs e)
        {
            SaveCurrentState();



            string selectedBoardName = ListBoard.SelectedItem as string;
            if (selectedBoardName != null)
            {
                LinkedList<TrelloList> listsForSelectedBoard = GetListsForSelectedBoard(selectedBoardName);
                if (listsForSelectedBoard != null)
                {
                    listCards.Items.Clear();
                    foreach (TrelloList list in listsForSelectedBoard)
                    {
                        listCards.Items.Add(list.Name);
                    }
                }
            }

        }
        private void DisplayLists()
        {
            listCards.Items.Clear(); 
            Cards.Items.Clear(); 

            foreach (TrelloList list in CurrentLists)
            {
                listCards.Items.Add(list.Name); 
            }
        }
        private void listCards_SelectedIndexChanged(object sender, EventArgs e)
        {
            SaveCurrentState();



            string selectedListName = listCards.SelectedItem as string;
            if (selectedListName != null)
            {
                LinkedList<TrelloCard> cardsForSelectedList = GetCardsForSelectedList(selectedListName);
                if (cardsForSelectedList != null)
                {
                    Cards.Items.Clear();
                    foreach (TrelloCard card in cardsForSelectedList)
                    {
                        Cards.Items.Add(card.Title);
                    }
                }
            }
        }
        private void SaveCurrentState()
        {

            CurrentLists.Clear();

           
            foreach (TrelloBoard board in ListBoards)
            {
                foreach (TrelloList list in board.Lists)
                {
                    CurrentLists.AddLast(list);
                }
            }
        }

            private void bttnAddBoard_Click(object sender, EventArgs e)
            {
                string newBoardName = txtBoard.Text;
                if (string.IsNullOrWhiteSpace(newBoardName))
                {
                    MessageBox.Show("Vui lòng nhập tên bảng trước khi thêm.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            TrelloBoard newBoard = new TrelloBoard(newBoardName);

            ListBoards.AddLast(newBoard);
            ListBoard.Items.Add(newBoardName);

                txtBoard.Text = " ";

            }
        
        private void btnDelBoards_Click(object sender, EventArgs e)
        {
            if (ListBoard.SelectedIndex != -1)
            {
                
                string selectedBoardName = ListBoard.SelectedItem as string;

                
                TrelloBoard boardToRemove = ListBoards.FirstOrDefault(board => board.Name == selectedBoardName);

                if (boardToRemove != null)
                {
                    
                    ListBoards.Remove(boardToRemove);
                }

                
                ListBoard.Items.RemoveAt(ListBoard.SelectedIndex);
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một mục để xóa.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnAddListCards_Click(object sender, EventArgs e)
        {
            if (ListBoard.SelectedIndex != -1)
            {
                string newCardName = textBListCards.Text;
                if (string.IsNullOrWhiteSpace(newCardName))
                {
                    MessageBox.Show("Vui lòng nhập tên thẻ trước khi thêm.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                string selectedBoardName = ListBoard.SelectedItem as string;

                TrelloList newList = new TrelloList(newCardName);
                foreach (TrelloBoard board in ListBoards)
                {
                    if (board.Name == selectedBoardName)
                    {
                        
                        board.AddList(newCardName);
                        break;
                    }
                }
                listCards.Items.Add(newCardName);

                textBListCards.Text = " ";
            }
            else
            {
                MessageBox.Show("Xin hãy chọn một bảng trong danh sách bảng", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        private void btnDelListCards_Click(object sender, EventArgs e)
        {
            if (listCards.SelectedIndex != -1)
            {
                
                string selectedListName = listCards.SelectedItem as string;

                
                TrelloList listToRemove = CurrentLists.FirstOrDefault(list => list.Name == selectedListName);

                if (listToRemove != null)
                {
                    
                    CurrentLists.Remove(listToRemove);
                }

                
                listCards.Items.RemoveAt(listCards.SelectedIndex);
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một mục để xóa.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnAddCards_Click(object sender, EventArgs e)
        {
            if (ListBoard.SelectedIndex == -1)
            {
                MessageBox.Show("Xin hãy chọn một bảng trong danh sách.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (listCards.SelectedIndex == -1)
            {
                MessageBox.Show("Xin hãy chọn danh sách thẻ bạn muốn xem.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            
            string selectedListName = listCards.SelectedItem.ToString();

            
            foreach (TrelloBoard board in ListBoards)
            {
                foreach (TrelloList list in board.Lists)
                {
                    if (list.Name == selectedListName)
                    {
                        
                        string newCardTitle = textBCards.Text.Trim();
                        if (!string.IsNullOrWhiteSpace(newCardTitle))
                        {
                            list.Cards.AddLast(new TrelloCard(newCardTitle));

                            
                            Cards.Items.Add(newCardTitle);

                           
                            textBCards.Text = "";
                        }
                        else
                        {
                            MessageBox.Show("Vui lòng nhập tên thẻ trước khi thêm.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        return; 
                    }
                }
            }

            
            MessageBox.Show("Không thể tìm thấy danh sách thẻ tương ứng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private void btnDelCards_Click(object sender, EventArgs e)
        {
            if (Cards.SelectedIndex != -1)
            {
                
                string selectedCardTitle = Cards.SelectedItem as string;

                
                string selectedListName = listCards.SelectedItem as string;

                
                TrelloList currentList = CurrentLists.FirstOrDefault(list => list.Name == selectedListName);

                
                if (currentList != null)
                {
                    
                    TrelloCard cardToRemove = currentList.Cards.FirstOrDefault(card => card.Title == selectedCardTitle);

                    
                    if (cardToRemove != null)
                    {
                        
                        currentList.Cards.Remove(cardToRemove);

                        
                        Cards.Items.RemoveAt(Cards.SelectedIndex);
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy thẻ để xóa.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Không tìm thấy danh sách thẻ.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một thẻ để xóa.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void Cards_SelectedIndexChanged(object sender, EventArgs e)
        {
            SaveCurrentState();

            
            string selectedListName = listCards.SelectedItem as string;

            
            foreach (TrelloBoard board in ListBoards)
            {
                foreach (TrelloList list in board.Lists)
                {
                    if (list.Name == selectedListName)
                    {
                       
                        Cards.Items.Clear();

                        
                        foreach (TrelloCard card in list.Cards)
                        {
                            Cards.Items.Add(card.Title);
                        }
                        return; 
                    }
                }
            }
        }
        private LinkedList<TrelloList> GetListsForSelectedBoard(string selectedBoardName)
        {
            foreach (TrelloBoard board in ListBoards)
            {
                if (board.Name == selectedBoardName)
                {
                    return board.Lists;
                }
            }
            return null;
        }
        private LinkedList<TrelloCard> GetCardsForSelectedList(string selectedListName)
        {
            foreach (TrelloList list in CurrentLists)
            {
                if (list.Name == selectedListName)
                {
                    return list.Cards;
                }
            }
            return null;
        }
        private LinkedList<TrelloCard> GetCardsForSelectedCard(string selectedListName)
        {
            foreach (TrelloBoard board in ListBoards)
            {
                foreach (TrelloList list in board.Lists)
                {
                    if (list.Name == selectedListName)
                    {
                        return list.Cards;
                    }
                }
            }
            return null;
        }
    }
}      
    

