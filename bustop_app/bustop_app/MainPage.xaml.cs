using bustop_app.ViewModel;
using MySql.Data.MySqlClient;
using System.Data;
using System.Diagnostics;
using bustop_app.Logics;

namespace bustop_app;

public partial class MainPage : ContentPage
{
    private businfor selectedItem;
    public MainPage(MainViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
		Title = "";
	}

    private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        selectedItem = (businfor)e.SelectedItem;
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
    }

    private async void Minuscnt_Clicked(object sender, EventArgs e)
    {
        string busCntString = selectedItem.Bus_cnt.Replace("명", "");
        int bus_cnt = int.Parse(busCntString);
        if (bus_cnt == 0)
        {
            await DisplayAlert("Warning", "탑승 대기 인원이 0명입니다. 취소가 불가능합니다!", "OK");
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
    }
}