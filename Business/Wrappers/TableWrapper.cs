
using Model.Models;

namespace Business.Wrappers
{
    public class TableWrapper : ModelWrapper<Table>
    {

        public int Id => Model.Id;

        public string Name
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public bool IsBusy
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public string OfficeName
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public int BranchOfficeId
        {
            get => GetValue<int>();
            set => SetValue(value);
        }

        public byte[] Image
        {
            get => GetValue<byte[]>();
            set => SetValue(value);
        }


        public TableWrapper(Table model) : base(model)
        {
            //Image = model.Image ?? ImageHelper.TableImg.ImgUrlToByteArray();
        }

        public void Update(Table table)
        {
            Name = table.Name;
            IsBusy = table.IsBussy;
            Image = table.Image;
        }
    }
}
