using bustop_app.ViewModel;
using MySql.Data.MySqlClient;
using System.Data;
using System.Diagnostics;
using bustop_app.Logics;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http.Json;
using Microsoft.Maui.Graphics;
namespace bustop_app;

public partial class MainPage : ContentPage
{
    HttpClient client = new HttpClient();//restAPI를 위함
    businforCollection busInfors = new businforCollection();//restAPI를 위함
    private businfor selectedItem;
    public MainPage()
	{
		InitializeComponent();
        MainViewModel vm = new MainViewModel();
		BindingContext = vm;
		Title = "";
        
    }

    private async void ContentPage_Loaded(object sender, EventArgs e)
    {
        client.BaseAddress = new Uri("http://210.119.12.69:7058/");//RestAPI 서버 기본 URL
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));//헤더설정
    }

    private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem == null)
        {
            return;
        }
        selectedItem = (businfor)e.SelectedItem;
    }

    //LoadDB는 MainPage.xaml.cs 에서 사용하기 위한 Search함수
    private async void LoadDB(object sender, EventArgs e)
    {
        MainViewModel vm = (MainViewModel)BindingContext;
        vm.Items.Clear();
        try
        {
            HttpResponseMessage? response = await client.GetAsync("api/BusTables");
            response.EnsureSuccessStatusCode();

            var busInfors = await response.Content.ReadAsAsync<Object>();
            var result = Newtonsoft.Json.JsonConvert.SerializeObject(busInfors);
            var jArray = JArray.Parse(result.ToString());
            foreach (var busInfo in jArray)
            {
                vm.Items.Add(new businfor
                {
                    Bus_idx = Int32.Parse(busInfo["busIdx"].ToString()),
                    Bus_num = busInfo["busNum"].ToString() + "번",
                    Bus_cnt = busInfo["busCnt"].ToString() + "명",
                    Bus_gap = busInfo["busGap"].ToString() + "분",
                    Bus_NowIn = busInfo["busNowIn"].ToString() + "명"
                });
            }
        }
        catch (Newtonsoft.Json.JsonException jEx)
        {
            Console.WriteLine(jEx.Message);
            //await(this.ShowMessageAsync("error", jEx.Message, MessageDialogStyle.Affirmative, new MetroDialogSettings()
            //{ AnimateShow = true, AnimateHide = true }));
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine(ex.Message);
            //await(this.ShowMessageAsync("error", ex.Message, MessageDialogStyle.Affirmative, new MetroDialogSettings()
            //{ AnimateShow = true, AnimateHide = true }));
        }

        //MainViewModel vm = (MainViewModel)BindingContext;
        //vm.Items.Clear();
        //using (MySqlConnection conn = new MySqlConnection(commons.myConnString))
        //{
        //    if (conn.State == ConnectionState.Closed)
        //        conn.Open();

        //    var query = @"SELECT * 
        //                      FROM bus_table 
        //                      ORDER BY bus_idx ASC";

        //    //MySqlCommand command = new MySqlCommand(query, conn);
        //    //MySqlDataReader reader = command.ExecuteReader();
        //    var cmd = new MySqlCommand(query, conn);
        //    var adapter = new MySqlDataAdapter(cmd);
        //    var dSet = new DataSet();
        //    adapter.Fill(dSet, "businfor");
        //    foreach (DataRow dr in dSet.Tables["businfor"].Rows)
        //    {
        //        vm.Items.Add(new businfor
        //        {
        //            Bus_idx = Convert.ToInt32(dr["bus_idx"]),
        //            Bus_num = Convert.ToString(dr["bus_num"]) + "번",
        //            Bus_cnt = Convert.ToInt32(dr["bus_cnt"]) + "명",
        //            Bus_gap = Convert.ToInt32(dr["bus_gap"]) + "분",
        //            Bus_NowIn = Convert.ToInt32(dr["bus_NowIn"]) + "명"
        //        });
        //    }
        //    conn.Close();
        //}
    }

    private async void Addcnt_Clicked(object sender, EventArgs e)
    {
        if(selectedItem==null)
        {
            await DisplayAlert("버스 미선택", "탑승하고자 하는 버스를 선택 후 버튼을 사용해주세요.", "확인");
            return;
        }

        string busCntString = selectedItem.Bus_cnt.Replace("명", "");
        string busNowInString = selectedItem.Bus_NowIn.Replace("명", "");
        

        int bus_count = int.Parse(busCntString) + int.Parse(busNowInString);

        if (bus_count>=50)
        {
            await DisplayAlert("최대 탑승 인원 초과", "탑승 인원은 50명을 초과할 수 없습니다!","확인");
            return;
        }
        //var strBus_idx = selectedItem.Bus_idx.ToString();
        //var AddBusCnt = $"api/BusTables/{selectedItem.Bus_idx}";
        try
        {
            // API 서버와 통신할 때 RestAPI 서버를 게시한 프로젝트 - Controllers / BusTables(DB명)Controller.cs의 클래스와 형태를 맞춰야 동작함
            var add_businfor = new BusTable()
            {
                BusIdx = selectedItem.Bus_idx,
                BusNum = selectedItem.Bus_num.Replace("번", "").ToString(),
                BusCnt = (Int32.Parse(selectedItem.Bus_cnt.Replace("명", "")) + 1),
                BusGap = Convert.ToInt32(selectedItem.Bus_gap.Replace("분", "")),
                BusNowIn = Convert.ToInt32(selectedItem.Bus_NowIn.Replace("명", ""))
            };
            //var content = new StringContent(JsonConvert.SerializeObject(add_businfor),Encoding.UTF8,"application/json");
            var response = await client.PutAsJsonAsync($"api/BusTables/{selectedItem.Bus_idx}",add_businfor);
            if(response.IsSuccessStatusCode)
            {
                await DisplayAlert("탑승 대기", $"{selectedItem.Bus_num} 버스 탑승 대기 완료!", "확인");
                LoadDB(sender, e);
                selectedItem = null;//선택된 아이템 해제
            }
            else
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                await DisplayAlert("실패", $"{errorResponse}", "확인");
            }
            //await DisplayAlert("탑승 대기", $"{selectedItem.Bus_num} 버스 탑승 대기 완료!", "확인");
            //LoadDB(sender, e);
        }
        catch (Newtonsoft.Json.JsonException jEx)
        {
            Console.WriteLine(jEx.Message);
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine(ex.Message);
        }
        //using (MySqlConnection conn = new MySqlConnection(commons.myConnString))
        //{
        //    if (conn.State == ConnectionState.Closed) conn.Open();
        //    var strBus_idx = selectedItem.Bus_idx.ToString();

        //    var query = @"UPDATE bus_table SET bus_cnt = bus_cnt+1 WHERE bus_idx = @strBus_idx";

        //    MySqlCommand cmd = new MySqlCommand(query, conn);
        //    cmd.Parameters.AddWithValue("@strBus_idx", strBus_idx);
        //    cmd.ExecuteNonQuery();
        //    conn.Close();
        //}
        //await DisplayAlert("탑승 대기", $"{selectedItem.Bus_num} 버스 탑승 대기 완료!", "확인");
        //LoadDB(sender,e);
    }

    private async void Minuscnt_Clicked(object sender, EventArgs e)
    {
        if (selectedItem == null)
        {
            await DisplayAlert("버스 미선택", "탑승하고자 하는 버스를 선택 후 버튼을 사용해주세요.", "확인");
            return;
        }

        string busCntString = selectedItem.Bus_cnt.Replace("명", "");
        int bus_cnt = int.Parse(busCntString);
        if (bus_cnt == 0)
        {
            await DisplayAlert("경고", "탑승 대기 인원이 0명입니다. 취소가 불가능합니다!", "확인");
            return;
        }
        try
        {
            // API 서버와 통신할 때 RestAPI 서버를 게시한 프로젝트 - Controllers / BusTables(DB명)Controller.cs의 클래스와 형태를 맞춰야 동작함
            var add_businfor = new BusTable()
            {
                BusIdx = selectedItem.Bus_idx,
                BusNum = selectedItem.Bus_num.Replace("번", "").ToString(),
                BusCnt = (Int32.Parse(selectedItem.Bus_cnt.Replace("명", "")) - 1),
                BusGap = Convert.ToInt32(selectedItem.Bus_gap.Replace("분", "")),
                BusNowIn = Convert.ToInt32(selectedItem.Bus_NowIn.Replace("명", ""))
            };
            //var content = new StringContent(JsonConvert.SerializeObject(add_businfor),Encoding.UTF8,"application/json");
            var response = await client.PutAsJsonAsync($"api/BusTables/{selectedItem.Bus_idx}", add_businfor);
            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("탑승 취소", $"{selectedItem.Bus_num} 버스 탑승 취소 완료!", "확인");
                LoadDB(sender, e);
                selectedItem = null;//선택된 아이템 해제
            }
            else
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                await DisplayAlert("실패", $"{errorResponse}", "확인");
            }
        }
        catch (Newtonsoft.Json.JsonException jEx)
        {
            Console.WriteLine(jEx.Message);
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine(ex.Message);
        }
        //using (MySqlConnection conn = new MySqlConnection(commons.myConnString))
        //{
        //    if (conn.State == ConnectionState.Closed) conn.Open();
        //    var strBus_idx = selectedItem.Bus_idx.ToString();

        //    var query = @"UPDATE bus_table SET bus_cnt = bus_cnt-1 WHERE bus_idx = @strBus_idx";

        //    MySqlCommand cmd = new MySqlCommand(query, conn);
        //    cmd.Parameters.AddWithValue("@strBus_idx", strBus_idx);
        //    cmd.ExecuteNonQuery();
        //    conn.Close();
        //}
    }
}