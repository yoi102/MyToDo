﻿using MyToDo.Common;
using MyToDo.Common.Events;
using Prism.Events;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Extensions
{
    public static class DialogExtensions
    {
        /// <summary>
        /// 询问窗口
        /// </summary>
        /// <param name="dialogHost">指定的dialogHost会话主机</param>
        /// <param name="title">标题</param>
        /// <param name="content">询问内容</param>
        /// <param name="dialogHostName">会话主机名称（唯一）</param>
        /// <returns></returns>
        public static async Task<IDialogResult> Question(this IDialogHostService dialogHost, string title, string content, string dialogHostName = "Root")
        {

            DialogParameters dialogParameters = new DialogParameters();

            dialogParameters.Add("Title", title);
            dialogParameters.Add("Content", content);
            dialogParameters.Add("DialogHostName", dialogHostName);
            var dialogResult = await dialogHost.ShowDialog("MsgView", dialogParameters, dialogHostName);
            return dialogResult;
        }




        /// <summary>
        /// 推送等待消息
        /// </summary>
        /// <param name="aggregator"></param>
        /// <param name="model"></param>
        public static void UpdataLoading(this IEventAggregator aggregator, UpdateModel model)
        {

            aggregator.GetEvent<UpdateLoadingEvent>().Publish(model);



        }


        /// <summary>
        /// 注册等待消息
        /// </summary>
        /// <param name="aggregator"></param>
        /// <param name="action"></param>
        public static void Register(this IEventAggregator aggregator, Action<UpdateModel> action)
        {

            aggregator.GetEvent<UpdateLoadingEvent>().Subscribe(action);



        }
        /// <summary>
        /// 注册提示消息事件
        /// </summary>
        /// <param name="aggregator"></param>
        /// <param name="action"></param>
        public static void RegisterMessage(this IEventAggregator aggregator, Action<MessageModel> action, string filterName = "Main")
        {

            aggregator.GetEvent<MessageEvent>().Subscribe(action,
                ThreadOption.PublisherThread, true, (m) =>
                {
                    return m.Filter.Equals(filterName);
                });
        }
        /// <summary>
        /// 发送提示消息
        /// </summary>
        /// <param name="aggregator"></param>
        /// <param name="message"></param>
        public static void SendMessage(this IEventAggregator aggregator, string message, string filterName = "Main")
        {

            aggregator.GetEvent<MessageEvent>().Publish(new MessageModel()
            {
                Filter = filterName,
                Message = message,
            });
        }








    }
}
