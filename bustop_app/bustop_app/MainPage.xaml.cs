using bustop_app.ViewModel;
using MySql.Data.MySqlClient;
using System.Data;
using System.Diagnostics;
using bustop_app.Logics;

namespace bustop_app;

public partial class MainPage : ContentPage
{
    private businfor selectedItem;
    public MainPage()
	{
		InitializeComponent();
        MainViewModel vm = new MainViewModel();
		BindingContext = vm;
		Title = "";
	}

    private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        selectedItem = (businfor)e.SelectedItem;
    }

    private void LoadDB(object sender, EventArgs e)
    {
        MainViewModel vm = (MainViewModel)BindingContext;
        vm.Items.Clear();
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
                vm.Items.Add(new businfor
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

    private async void Addcnt_Clicked(object sender, EventArgs e)
    {
        string busCntString = selectedItem.Bus_cnt.Replace("명", "");
        string busNowInString = selectedItem.Bus_NowIn.Replace("명", "");

        int bus_count = int.Parse(busCntString) + int.Parse(busNowInString);

        if (bus_count>=50)
        {
            await DisplayAlert("Warning", "탑승 인원은 50명을 초과할 수 없습니다!","OK");
            return;
        }
        using (MySqlConnection conn = new MySqlConnection(commons.myConnString))
        {
            if (conn.State == ConnectionState.Closed) conn.Open();
            var strBus_idx = selectedItem.Bus_idx.ToString();

            var query = @"UPDATE bus_table SET bus_cnt = bus_cnt+1 WHERE bus_idx = @strBus_idx";

            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@strBus_idx", strBus_idx);
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        await DisplayAlert("탑승 대기", $"{selectedItem.Bus_num} 버스 탑승 대기 완료!", "확인");
        LoadDB(sender,e);
    }

    private async void Minuscnt_Clicked(object sender, EventArgs e)
    {
        string busCntString = selectedItem.Bus_cnt.Replace("명", "");
        int bus_cnt = int.Parse(busCntString);
        if (bus_cnt == 0)
        {
            await DisplayAlert("경고", "탑승 대기 인원이 0명입니다. 취소가 불가능합니다!", "확인");
            return;
        }
        using (MySqlConnection conn = new MySqlConnection(commons.myConnString))
        {
            if (conn.State == ConnectionState.Closed) conn.Open();
            var strBus_idx = selectedItem.Bus_idx.ToString();

            var query = @"UPDATE bus_table SET bus_cnt = bus_cnt-1 WHERE bus_idx = @strBus_idx";

            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@strBus_idx", strBus_idx);
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        await DisplayAlert("탑승 취소", $"{selectedItem.Bus_num} 버스 탑승 취소 완료!", "확인");
        LoadDB(sender, e);
    }
}