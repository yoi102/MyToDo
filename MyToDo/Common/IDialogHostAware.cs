﻿using Prism.Commands;
using Prism.Services.Dialogs;

namespace MyToDo.Common
{
    public interface IDialogHostAware
    {
        /// <summary>
        /// 所属DialogHost名称
        /// </summary>
        string DialogHostName { get; set; }

        /// <summary>
        /// 打开过程中执行
        /// </summary>
        /// <param name="parameters"></param>
        void OnDialogOpend(IDialogParameters parameters);

        DelegateCommand SaveCommand { get; set; }
        DelegateCommand CancelCommand { get; set; }
    }
}