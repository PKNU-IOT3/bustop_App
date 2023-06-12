using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;

namespace bustop_app.ViewModel
{
    public class businforCollection : ObservableCollection<businfor>
    {
        public void CopyForm(IEnumerable<businfor> businfors) 
        {
            this.Items.Clear();//초기화
            foreach(businfor item in  businfors)
            {
                this.Items.Add(item);//데이터 추가
            }
            //데이터 변경을 알림
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
    }
}
