﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using MySql.Data.MySqlClient;
using bustop_app.Logics;
using System.Data;
using System.Windows.Input;

namespace bustop_app.ViewModel
{
    public partial class MainViewModel : ObservableObject
    {
        public MainViewModel()
        {
            Items = new ObservableCollection<businfor>();
        }

        private ObservableCollection<businfor> items;
        public ObservableCollection<businfor> Items
        {
            get => items;
            set => SetProperty(ref items, value);
        }

        private string text;
        public string Text
        {
            get => text;
            set => SetProperty(ref text, value);
        }

        private int bus_idx;
        public int Bus_idx
        {
            get => bus_idx;
            set=>SetProperty(ref bus_idx, value);
        }

        private string bus_num;
        public string Bus_num
        {
            get=> bus_num;
            set=>SetProperty(ref bus_num, value);
        }

        private string bus_cnt;
        public string Bus_cnt
        {
            get=>bus_cnt;
            set => SetProperty(ref bus_cnt, value);
        }

        private string bus_gap;
        public string Bus_gap
        {
            get => bus_gap;
            set => SetProperty(ref bus_gap, value);
        }

        private string bus_NowIn;
        public string Bus_NowIn
        {
            get => bus_NowIn; 
            set => SetProperty(ref bus_NowIn, value);
        }

        private bool isHeaderVisible;
        public bool IsHeaderVisible
        {
            get => isHeaderVisible;
            set => SetProperty(ref isHeaderVisible, value);
        }

        public ICommand SearchCommand => new RelayCommand(Search);
        public ICommand AddCnt => new RelayCommand(Add);
        public ICommand MinusCnt => new RelayCommand(Minus);

        private void Search()
        {
            IsHeaderVisible = true;
            Items.Clear();
            //List<businfor> list = new List<businfor>();
            using (MySqlConnection conn = new MySqlConnection(commons.myConnString))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                var query = @"SELECT * 
                              FROM bus_table 
                              ORDER BY bus_idx ASC";

                //MySqlCommand command = new MySqlCommand(query, conn);
                //MySqlDataReader reader = command.ExecuteReader();
                var cmd = new MySqlCommand(query, conn);
                var adapter = new MySqlDataAdapter(cmd);
                var dSet = new DataSet();
                adapter.Fill(dSet, "businfor");
                foreach (DataRow dr in dSet.Tables["businfor"].Rows)
                {
                    Items.Add(new businfor
                    {
                        Bus_idx = Convert.ToInt32(dr["bus_idx"]),
                        Bus_num = Convert.ToString(dr["bus_num"]) + "번",
                        Bus_cnt = Convert.ToInt32(dr["bus_cnt"]) + "명",
                        Bus_gap = Convert.ToInt32(dr["bus_gap"]) + "분",
                        Bus_NowIn = Convert.ToInt32(dr["bus_NowIn"]) + "명"
                    });
                }
                conn.Close();
            }
        }
        //탑승 대기 로직
        private void Add()
        {

        }
        //탑승 취소 로직
        private void Minus()
        {

        }

        public ICommand DeleteCommand => new RelayCommand<string>(Delete);

        private void Delete(string s)
        {
            //if (Items.Contains(s))
            //{
            //    Items.Remove(s);
            //}
        }
    }
}