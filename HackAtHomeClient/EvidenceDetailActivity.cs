﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using HackAtHome.SAL;
using HackAtHomeClient.Fragments;
using Android.Webkit;
using WebView = Android.Webkit.WebView;

namespace HackAtHomeClient
{
	[Activity(Label = "@string/ApplicationName")]
	public class EvidenceDetailActivity : Activity
	{
		private EvidenceDetailFragment Data;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			SetContentView(Resource.Layout.EvidenceDetail);

			// Utiliza el Fragment Manager para recuperar el fragmento
			Data = (EvidenceDetailFragment)this.FragmentManager.FindFragmentByTag("Data");
			if (Data == null)
			{
				// No ha sido almacenado, agregar el fragmento a la Activity
				Data = new EvidenceDetailFragment();
				var FragmentTransaction = this.FragmentManager.BeginTransaction();
				FragmentTransaction.Add(Data, "Data");
				FragmentTransaction.Commit();
			}
	
			LoadData();
		}

		/// <summary>
		/// Persistencia de la data.
		/// </summary>
		private async void LoadData()
		{
			// Si no existe un token intentamos recargar la informacion
			if (Data.Evidence == null)
			{
				// Recuperamos la informaicon pasada desde la otra actividad
				Data.FullName = Intent.GetStringExtra("FullName");
				Data.Token = Intent.GetStringExtra("Token");

				Data.Evidence = new HackAtHome.Entities.Evidence()
				{
					EvidenceID = Intent.GetIntExtra("EvidenceID", 0),
					Title = Intent.GetStringExtra("EvidenceTitle"),
					Status = Intent.GetStringExtra("EvidenceStatus")
				};

				// Recuperamos la lista de las evidencias
				var ServiceClient = new ServiceClient();

				try
				{
					Data.Detail = await ServiceClient.GetEvidenceByIdAsync(Data.Token, Data.Evidence.EvidenceID);
				}
				catch (Exception ex)
				{
					// Creamos un dialogo para mostrar la excepcion
					ErrorDialog(ex.Message);
					Data.Detail = new HackAtHome.Entities.EvidenceDetail();
				}
			}

			// Cargamos la informacion en el layout
			var FullNameTextView = FindViewById<TextView>(Resource.Id.tvDetailFullName);
			FullNameTextView.Text = Data.FullName;

			var TitleTextView = FindViewById<TextView>(Resource.Id.tvDetailTitle);
			TitleTextView.Text = Data.Evidence.Title;

			var StatusTextView = FindViewById<TextView>(Resource.Id.tvDetailStatus);
			StatusTextView.Text = Data.Evidence.Status;

         
            string WebViewContent = $"<div style='color:#BCBCBC;'>{Data.Detail.Description}</div>";
			var DescriptionWebView = FindViewById<WebView>(Resource.Id.webViewContent);
			DescriptionWebView.LoadDataWithBaseURL(null, WebViewContent, "text/html", "utf-8", null);
			DescriptionWebView.SetBackgroundColor(Android.Graphics.Color.Transparent);

			var EvidenceImageView = FindViewById<ImageView>(Resource.Id.imageViewEvidence);
			Koush.UrlImageViewHelper.SetUrlDrawable(EvidenceImageView, Data.Detail.Url);

          
		}

        private void ErrorDialog(string title = "Error", int iconResource = Resource.Drawable.Icon, string message = "Ocurrio un error inesperado")
		{
			Android.App.AlertDialog.Builder Builder = new AlertDialog.Builder(this);
			AlertDialog Alert = Builder.Create();
			Alert.SetTitle(title);
			Alert.SetIcon(iconResource);
			Alert.SetMessage($"Ocurrio un error inesperado:\n{message}");
			Alert.SetButton("Ok", (s, ev) => { });
			Alert.Show();
		}
	}
}