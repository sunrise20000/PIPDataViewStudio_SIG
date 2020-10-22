using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace PIPDataViewStudio.CLass
{
	
		public class DetailedDateTime : UITypeEditor
		{

			IWindowsFormsEditorService editorService;
			DateTimePicker picker = new DateTimePicker();

			public DetailedDateTime()
			{
				picker.Format = DateTimePickerFormat.Custom;
				picker.CustomFormat = "dd/MM/yyyy HH:mm:ss";
			}

			public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
			{
				return UITypeEditorEditStyle.DropDown;
			}

			public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
			{
				if (provider != null)
				{
					this.editorService = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
				}

				if (this.editorService != null)
				{
					picker.Value = DateTime.Parse(value.ToString());
					this.editorService.DropDownControl(picker);
					value = picker.Value;
				}

				return value;
			}
		}
	
}
