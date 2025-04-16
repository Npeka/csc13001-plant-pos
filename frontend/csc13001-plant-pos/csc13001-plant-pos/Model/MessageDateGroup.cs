using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace csc13001_plant_pos.Model
{
    public class MessageDateGroup
    {
        public DateTime Date { get; set; }
        public ObservableCollection<Message> Messages { get; set; }

        public MessageDateGroup(DateTime date, IEnumerable<Message> messages)
        {
            Date = date;
            Messages = new ObservableCollection<Message>(messages);
        }
    }
}