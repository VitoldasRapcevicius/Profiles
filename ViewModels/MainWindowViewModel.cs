using Profiles.Commands;
using Profiles.Models;
using Profiles.Service;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Profiles.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        //Create projects list and bind with dataGrid
        public ObservableCollection<Project> Projects { get; set; } = new();
        //Create propertyType list and bind with dataGrid
        public ObservableCollection<PropertyType> PropertyTypes { get; set; } = new();
        //Create Properties list and bind with dataGrid
        public ObservableCollection<Propertie> Properties { get; set; } = new();
        //Create SubProperties list and bind with dataGrid
        public ObservableCollection<SubProperty> SubProperties { get; set; } = new();

        //Create object based on DataBaseService class to access methods.
        private DataBaseService _dataBaseService { get; set; } = new();

        public Project Project { get; set; } = new();
        public PropertyType PropertyType { get; set; } = new();
        public Propertie Property { get; set; } = new();
        public SubProperty SubProperty { get; set; } = new();

        //Create command to call method create project in DB
        public ICommand CreateProjectCommand { get; set; }
        //Create command to call method create propertyType in DB
        public ICommand CreatePropertyTypeCommand { get; set; }
        //Create command to call method create property in DB
        public ICommand CreatePropertyCommand { get; set; }
        //Create command to call method create subProperty in DB
        public ICommand CreateSubPropertyCommand { get; set; }

        public ICommand DeleteProjectCommand { get; set; }
        public ICommand DeletePropertyTypeCommand { get; set; }
        public ICommand DeletePropertyCommand { get; set; }
        public ICommand DeleteSubPropertyCommand { get; set; }

        public ICommand UpdateProjectCommand { get; set; }
        public ICommand UpdatePropertyTypeCommand { get; set; }
        public ICommand UpdatePropertyCommand { get; set; }
        public ICommand UpdateSubPropertyCommand { get; set; }


        Project _selectedProject;
        PropertyType _selectedPropertyType;
        Propertie _selectedProperty;
        SubProperty _selectedSubProperty;

        public SubProperty SelectedSubProperty
        {
            get { return _selectedSubProperty; }
            set { _selectedSubProperty = value; }
        }

        public Propertie SelectedProperty
        {
            get { return _selectedProperty; }
            set
            {
                _selectedProperty = value;
                if (_selectedProperty != null)
                {
                    SubProperties.Clear();
                    //Getting SubProperties list form DB and binding with the corresponding comboBox
                    foreach (var subProperty in _dataBaseService.GetSubPropertiesByPropertyID(_selectedProperty.Id))
                    {
                        SubProperties.Add(subProperty);
                    }
                    //Impact dataGrid and changed data in the table
                    OnPorpertyChanged(nameof(SelectedProperty));
                    OnPorpertyChanged(nameof(SubProperties));
                }
            }
        }

        public PropertyType SelectedPropertyType
        {
            get { return _selectedPropertyType; }
            set
            {
                _selectedPropertyType = value;
                //Prevent to get nullable value from _selectedPropertyType and avoid crashing method GetPropertiesByProjectIDPropertyTypeID(_selectedProject.Id, _selectedPropertyType.Id)
                if (_selectedPropertyType != null && _selectedProject != null)
                {
                    Properties.Clear();
                    //Getting Properties list form DB and binding with the corresponding comboBox
                    foreach (var property in _dataBaseService.GetPropertiesByProjectIDPropertyTypeID(_selectedProject.Id, _selectedPropertyType.Id))
                    {
                        Properties.Add(property);
                    }
                    //Impact dataGrid and changed data in the table
                    OnPorpertyChanged(nameof(Properties));
                }
            }
        }

        public Project SelectedProject
        {
            get
            {
                return _selectedProject;
            }
            set
            {
                _selectedProject = value;
                //Prevent to get nullable value from _selectedProject and avoid crashing method GetPropertyTypesByProjectID(_selectedProject.Id)
                if (_selectedProject != null)
                {
                    PropertyTypes.Clear();
                    //Getting PropertyType list from DB and binding with the corresponding comboBox
                    foreach (var propertyType in _dataBaseService.GetPropertyTypesByProjectID(_selectedProject.Id))
                    {
                        PropertyTypes.Add(propertyType);
                    }
                    //Impact dataGrid and changed data in the table
                    OnPorpertyChanged(nameof(PropertyTypes));
                }
            }
        }

        public MainWindowViewModel()
        {
            //Create a commands to bind methods with buttons
            CreateProjectCommand = new RelayCommand(param => CreateProjectButton(), param => true);
            CreatePropertyTypeCommand = new RelayCommand(param => CreatePropertyTypeButton(), param => true);
            CreatePropertyCommand = new RelayCommand(param => CreatePropertyButton(), param => true);
            CreateSubPropertyCommand = new RelayCommand(param => CreateSubPropertyButton(), param => true);

            DeleteProjectCommand = new RelayCommand(param => DeleteProjectButton(), param => true);
            DeletePropertyTypeCommand = new RelayCommand(param => DeletePropertyTypeButton(), param => true);
            DeletePropertyCommand = new RelayCommand(param => DeletePropertyButton(), param => true);
            DeleteSubPropertyCommand = new RelayCommand(param => DeleteSubPropertyButton(), param => true);

            UpdateProjectCommand = new RelayCommand(param => UpdateProjectButton(), param => true);
            UpdatePropertyTypeCommand = new RelayCommand(param => UpdatePropertyTypeButton(), param => true);
            UpdatePropertyCommand = new RelayCommand(param => UpdatePropertyButton(), param => true);
            UpdateSubPropertyCommand = new RelayCommand(param => UpdateSubPropertyButton(), param => true);

            //Getting projects list from DB. Converting to ObservableCollection and binding with the corresponding comboBox
            foreach (var project in _dataBaseService.GetProjects())
            {
                Projects.Add(project);
            }
        }

        private void UpdateProjectButton()
        {
            if (Project.InternalId > 0 && !string.IsNullOrEmpty(Project.Name) && _selectedProject != null)
            {
                _dataBaseService.UpdateProjectByID(Project, _selectedProject.Id);
                LoadListProjects();
                MessageBox.Show("Successfully updated");
            }
            else
            {
                string errorMessage = "";
                if (Project.InternalId <= 0)
                    errorMessage += "Please write a valid InternalId\n";

                if (string.IsNullOrEmpty(Project.Name))
                    errorMessage += "Please write a Name\n";

                if (_selectedProject == null)
                    errorMessage += "Please select a Project\n";
                MessageBox.Show(errorMessage.TrimEnd('\n'));
            }
        }

        private void UpdatePropertyTypeButton()
        {
            if (!string.IsNullOrEmpty(PropertyType.Name) && _selectedPropertyType != null && _selectedProject != null)
            {
                _dataBaseService.UpdatePropertyTypeByID(PropertyType, _selectedPropertyType.Id, _selectedProject.Id);
                LoadListPropertyTypes();
                MessageBox.Show("Successfully updated");
            }
            else
            {
                string errorMessage = "";
                if (string.IsNullOrEmpty(PropertyType.Name))
                    errorMessage += "Please write a Name\n";

                if (_selectedPropertyType == null)
                    errorMessage += "Please select a PropertyType\n";

                if (_selectedProject == null)
                    errorMessage += "Please select a Project\n";
                MessageBox.Show(errorMessage.TrimEnd('\n'));
            }
        }

        private void UpdatePropertyButton()
        {
            if (!string.IsNullOrEmpty(Property.Name) && !string.IsNullOrEmpty(Property.Prefix) && _selectedPropertyType != null && _selectedProject != null && _selectedProperty != null)
            {
                _dataBaseService.UpdatePropertyByID(Property, _selectedPropertyType.Id, _selectedProject.Id, _selectedProperty.Id);
                LoadListProperties();
                MessageBox.Show("Successfully updated");
            }
            else
            {
                string errorMessage = "";
                if (string.IsNullOrEmpty(Property.Name))
                    errorMessage += "Please write a Name\n";

                if (string.IsNullOrEmpty(Property.Prefix))
                    errorMessage += "Please write a Prefix\n";

                if (_selectedPropertyType == null)
                    errorMessage += "Please select a PropertyType\n";

                if (_selectedProject == null)
                    errorMessage += "Please select a Project\n";

                if (_selectedProperty == null)
                    errorMessage += "Please select a Property\n";
                MessageBox.Show(errorMessage.TrimEnd('\n'));
            }
        }

        private void UpdateSubPropertyButton()
        {
            if (!string.IsNullOrEmpty(SubProperty.Name) && !string.IsNullOrEmpty(SubProperty.Prefix) && _selectedProperty != null && _selectedSubProperty != null)
            {
                _dataBaseService.UpdateSubPropertyByID(SubProperty, _selectedProperty.Id, _selectedSubProperty.Id);
                LoadListSubProperties();
                MessageBox.Show("Successfully updated");
            }
            else
            {
                string errorMessage = "";
                if (string.IsNullOrEmpty(SubProperty.Name))
                    errorMessage += "Please write a Name\n";

                if (string.IsNullOrEmpty(SubProperty.Prefix))
                    errorMessage += "Please write a Prefix\n";

                if (_selectedProperty == null)
                    errorMessage += "Please select a Property\n";

                if (_selectedSubProperty == null)
                    errorMessage += "Please select a SubProperty\n";
                MessageBox.Show(errorMessage.TrimEnd('\n'));
            }
        }

        private void DeleteProjectButton()
        {
            if (_selectedProject != null)
            {
                if (MessageBox.Show("Are you sore to delete?", "Delete record", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    _dataBaseService.DeleteProjectByID(_selectedProject.Id);
                    LoadListProjects();
                    MessageBox.Show("Successfully deleted");
                }
            }
            else
            {
                MessageBox.Show("Please, select the Project");
            }
        }

        private void DeletePropertyTypeButton()
        {
            if (_selectedPropertyType != null)
            {
                if (MessageBox.Show("Are you sore to delete?", "Delete record", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    _dataBaseService.DeletePropertyTypeByID(_selectedPropertyType.Id);
                    LoadListPropertyTypes();
                    MessageBox.Show("Successfully deleted");
                }
            }
            else
            {
                MessageBox.Show("Please, select the PropertyType");
            }
        }

        private void DeletePropertyButton()
        {
            if (_selectedProperty != null)
            {
                if (MessageBox.Show("Are you sore to delete?", "Delete record", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    _dataBaseService.DeletePropertyByID(_selectedProperty.Id);
                    if (_selectedPropertyType != null && _selectedProject != null)
                    {
                        LoadListProperties();
                        MessageBox.Show("Successfully deleted");
                    }
                }
            }
            else
            {
                MessageBox.Show("Please, select the Property");
            }
        }

        private void DeleteSubPropertyButton()
        {
            if (_selectedSubProperty != null)
            {
                if (MessageBox.Show("Are you sore to delete?", "Delete record", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    _dataBaseService.DeleteSubPropertyByID(_selectedSubProperty.Id);
                    LoadListSubProperties();
                    MessageBox.Show("Successfully deleted");
                }
            }
            else
            {
                MessageBox.Show("Please, select the SubProperty");
            }
        }

        private void CreateProjectButton()
        {
            if (Project.InternalId > 0 && !string.IsNullOrEmpty(Project.Name))
            {
                _dataBaseService.CreateProject(Project);
                LoadListProjects();
                MessageBox.Show("Successfully created");
            }
            else
            {
                string errorMessage = "";
                if (Project.InternalId <= 0)
                    errorMessage += "Please write a valid InternalId\n";

                if (string.IsNullOrEmpty(Project.Name))
                    errorMessage += "Please write a Name\n";
                MessageBox.Show(errorMessage.TrimEnd('\n'));
            }
        }

        private void CreatePropertyTypeButton()
        {
            if (!string.IsNullOrEmpty(PropertyType.Name) && _selectedProject != null)
            {
                _dataBaseService.CreatePropertyType(PropertyType, _selectedProject.Id);
                LoadListPropertyTypes();
                MessageBox.Show("Successfully created");
            }
            else
            {
                string errorMessage = "";
                if (string.IsNullOrEmpty(PropertyType.Name))
                    errorMessage += "Please write a Name\n";

                if (_selectedProject == null)
                    errorMessage += "Please select a Project\n";
                MessageBox.Show(errorMessage.TrimEnd('\n'));
            }
        }

        private void CreatePropertyButton()
        {
            if (!string.IsNullOrEmpty(Property.Name) && !string.IsNullOrEmpty(Property.Prefix) && _selectedProject != null && _selectedPropertyType != null)
            {
                _dataBaseService.CreateProperty(Property, _selectedProject.Id, _selectedPropertyType.Id);
                LoadListProperties();
                MessageBox.Show("Successfully created");
            }
            else
            {
                string errorMessage = "";
                if (string.IsNullOrEmpty(Property.Name))
                    errorMessage += "Please write a Name\n";

                if (string.IsNullOrEmpty(Property.Prefix))
                    errorMessage += "Please write a Prefix\n";

                if (_selectedProject == null)
                    errorMessage += "Please select a Project\n";

                if (_selectedPropertyType == null)
                    errorMessage += "Please select a PropertyType\n";
                MessageBox.Show(errorMessage.TrimEnd('\n'));
            }
        }

        private void CreateSubPropertyButton()
        {
            if (!string.IsNullOrEmpty(SubProperty.Name) && !string.IsNullOrEmpty(SubProperty.Prefix) && _selectedProperty != null)
            {
                _dataBaseService.CreateSubProperty(SubProperty, _selectedProperty.Id);
                LoadListSubProperties();
                MessageBox.Show("Successfully created");
            }
            else
            {
                string errorMessage = "";
                if (string.IsNullOrEmpty(SubProperty.Name))
                    errorMessage += "Please write a Name\n";

                if (string.IsNullOrEmpty(SubProperty.Prefix))
                    errorMessage += "Please write a Prefix\n";

                if (_selectedProperty == null)
                    errorMessage += "Please select a Property\n";
                MessageBox.Show(errorMessage.TrimEnd('\n'));
            }
        }

        public void LoadListProjects()
        {
            //Clear projects list from the dataGrid
            Projects.Clear();
            // Get all projects from DB and assign them to the Projects collection
            foreach (var project in _dataBaseService.GetProjects())
            {
                Projects.Add(project);
            }
            //Makes filled fields empty
            Project = new();
            //Impact dataGrid and changed data in the table
            OnPorpertyChanged(nameof(Project));
        }

        public void LoadListPropertyTypes()
        {
            //Clear propertyType list from the dataGrid
            PropertyTypes.Clear();
            // Get all propertyTypes from DB and assign them to the PropertyTypes collection
            foreach (var propertyType in _dataBaseService.GetPropertyTypesByProjectID(_selectedProject.Id))
            {
                PropertyTypes.Add(propertyType);
            }
            //Makes filled fields empty
            PropertyType = new();
            //Impact dataGrid and changed data in the table
            OnPorpertyChanged(nameof(PropertyType));
        }

        public void LoadListProperties()
        {
            //Clear property list from the dataGrid
            Properties.Clear();
            // Get all properties from DB and assign them to the Properties collection
            foreach (var property in _dataBaseService.GetPropertiesByProjectIDPropertyTypeID(_selectedProject.Id, _selectedPropertyType.Id))
            {
                Properties.Add(property);
            }
            //Makes filled fields empty
            Property = new();
            //Impact dataGrid and changed data in the table
            OnPorpertyChanged(nameof(Property));
        }

        public void LoadListSubProperties()
        {
            // Clear subProperty list from the dataGrid
            SubProperties.Clear();
            //Getting SubProperties list form DB and binding with the corresponding comboBox
            foreach (var subProperty in _dataBaseService.GetSubPropertiesByPropertyID(_selectedProperty.Id))
            {
                SubProperties.Add(subProperty);
            }
            //Makes filled fields empty
            SubProperty = new();
            //Impact dataGrid and changed data in the table
            OnPorpertyChanged(nameof(SubProperty));
        }
    }
}