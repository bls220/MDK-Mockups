using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace IngameScript.Views
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged([CallerMemberName] string property = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));


        /// <summary>
        /// Gets called whenever a property in the model is changed. We use this to tell when our mock was updated.
        /// </summary>
        /// <remarks>You only need to override and add an entry if the properties' name is different from the corresponding model property.
        /// If anyone knows of a better way to update the ViewModel when the mock changes, let us know.</remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                default:
                    // Forward event
                    this.RaisePropertyChanged(e.PropertyName);
                    break;
            }
        }
    }
}
