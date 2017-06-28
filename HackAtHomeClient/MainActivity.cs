using Android.App;
using Android.Widget;
using Android.OS;
using HackAtHome.SAL;
using HackAtHome.Entities;



namespace HackAtHomeClient
{
    [Activity(Label = "@string/ApplicationName", MainLauncher = true, Icon = "@drawable/xamarin", Theme = "@android:style/Theme.Holo")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView (Resource.Layout.Main);
            var validateButton = FindViewById<Button>(Resource.Id.button1);
			validateButton.Click += (s, ev) =>
			{
				Validate();
			};
        }

        private async void ValidateEvidence(string email)
        {
            var MicrosoftEvidence = new LabItem
            {
                Email = email,
                Lab = "Hack@Home",
                DeviceId = Android.Provider.Settings.Secure.GetString(ContentResolver, Android.Provider.Settings.Secure.AndroidId)
            };

            var MicrosftClient = new MicrosoftServiceClient();
            await MicrosftClient.SendEvidence(MicrosoftEvidence);
        }

		private async void Validate()
		{
			ServiceClient ServiceClient = new ServiceClient();
			var emailEditText = FindViewById<EditText>(Resource.Id.editTextEmail);
			var passwordEditText = FindViewById<EditText>(Resource.Id.editTextPassword);

			ResultInfo Result = await ServiceClient.AutenticateAsync(emailEditText.Text, passwordEditText.Text);

			// Creamos un dialogo para mostrar el resultado de la validacion
			Android.App.AlertDialog.Builder Builder = new AlertDialog.Builder(this);
			AlertDialog Alert = Builder.Create();
			Alert.SetTitle("Resultado de la Validación");

			// Mostramos un mensaje personalizado
			string message;

			if (Result.Status == Status.Success || Result.Status == Status.AllSuccess)
			{

                //enviamos la evidencia 
				ValidateEvidence(emailEditText.Text);
				message = $"Bienvenido {Result.FullName}";
			}
			else
			{
				message = $"Error:\n{Result.Status}\n{Result.FullName}";
			}

			//si todo fue satisfactorio pasamos a la otra actividad
			Alert.SetMessage(message);
			Alert.SetButton("Ok", (s, ev) =>
			{
				if (Result.Status == Status.Success || Result.Status == Status.AllSuccess)
				{
					var Intent = new Android.Content.Intent(this, typeof(EvidenceListActivity));
					Intent.PutExtra("FullName", Result.FullName);
					Intent.PutExtra("Token", Result.Token);
					StartActivity(Intent);
				}
			});
			Alert.Show();
		}
	}
    }


