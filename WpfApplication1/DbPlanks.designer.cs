﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WpfApplication1
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="SortareCherestea")]
	public partial class DbPlanksDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void InsertPlancks(Plancks instance);
    partial void UpdatePlancks(Plancks instance);
    partial void DeletePlancks(Plancks instance);
    #endregion
		
		public DbPlanksDataContext() : 
				base(global::WpfApplication1.Properties.Settings.Default.SortareCheresteaConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public DbPlanksDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public DbPlanksDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public DbPlanksDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public DbPlanksDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<Plancks> Plancks
		{
			get
			{
				return this.GetTable<Plancks>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.Plancks")]
	public partial class Plancks : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _Id;
		
		private decimal _bfActualLength;
		
		private decimal _bfActualWidth;
		
		private decimal _bfActualThickness;
		
		private string _bfQalClass;
		
		private string _bfPalletId;
		
		private System.Nullable<System.DateTime> _timeStamp;
		
		private string _bfPlanckId;
		
		private string _bfProductId;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIdChanging(int value);
    partial void OnIdChanged();
    partial void OnbfActualLengthChanging(decimal value);
    partial void OnbfActualLengthChanged();
    partial void OnbfActualWidthChanging(decimal value);
    partial void OnbfActualWidthChanged();
    partial void OnbfActualThicknessChanging(decimal value);
    partial void OnbfActualThicknessChanged();
    partial void OnbfQalClassChanging(string value);
    partial void OnbfQalClassChanged();
    partial void OnbfPalletIdChanging(string value);
    partial void OnbfPalletIdChanged();
    partial void OntimeStampChanging(System.Nullable<System.DateTime> value);
    partial void OntimeStampChanged();
    partial void OnbfPlanckIdChanging(string value);
    partial void OnbfPlanckIdChanged();
    partial void OnbfProductIdChanging(string value);
    partial void OnbfProductIdChanged();
    #endregion
		
		public Plancks()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Id", DbType="Int NOT NULL", IsPrimaryKey=true, IsDbGenerated=true)]
		public int Id
		{
			get
			{
				return this._Id;
			}
			set
			{
				if ((this._Id != value))
				{
					this.OnIdChanging(value);
					this.SendPropertyChanging();
					this._Id = value;
					this.SendPropertyChanged("Id");
					this.OnIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_bfActualLength", DbType="Decimal(18,0) NOT NULL")]
		public decimal bfActualLength
		{
			get
			{
				return this._bfActualLength;
			}
			set
			{
				if ((this._bfActualLength != value))
				{
					this.OnbfActualLengthChanging(value);
					this.SendPropertyChanging();
					this._bfActualLength = value;
					this.SendPropertyChanged("bfActualLength");
					this.OnbfActualLengthChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_bfActualWidth", DbType="Decimal(18,0) NOT NULL")]
		public decimal bfActualWidth
		{
			get
			{
				return this._bfActualWidth;
			}
			set
			{
				if ((this._bfActualWidth != value))
				{
					this.OnbfActualWidthChanging(value);
					this.SendPropertyChanging();
					this._bfActualWidth = value;
					this.SendPropertyChanged("bfActualWidth");
					this.OnbfActualWidthChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_bfActualThickness", DbType="Decimal(18,0) NOT NULL")]
		public decimal bfActualThickness
		{
			get
			{
				return this._bfActualThickness;
			}
			set
			{
				if ((this._bfActualThickness != value))
				{
					this.OnbfActualThicknessChanging(value);
					this.SendPropertyChanging();
					this._bfActualThickness = value;
					this.SendPropertyChanged("bfActualThickness");
					this.OnbfActualThicknessChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_bfQalClass", DbType="NChar(10) NOT NULL", CanBeNull=false)]
		public string bfQalClass
		{
			get
			{
				return this._bfQalClass;
			}
			set
			{
				if ((this._bfQalClass != value))
				{
					this.OnbfQalClassChanging(value);
					this.SendPropertyChanging();
					this._bfQalClass = value;
					this.SendPropertyChanged("bfQalClass");
					this.OnbfQalClassChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_bfPalletId", DbType="Text", UpdateCheck=UpdateCheck.Never)]
		public string bfPalletId
		{
			get
			{
				return this._bfPalletId;
			}
			set
			{
				if ((this._bfPalletId != value))
				{
					this.OnbfPalletIdChanging(value);
					this.SendPropertyChanging();
					this._bfPalletId = value;
					this.SendPropertyChanged("bfPalletId");
					this.OnbfPalletIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_timeStamp", DbType="DateTime")]
		public System.Nullable<System.DateTime> timeStamp
		{
			get
			{
				return this._timeStamp;
			}
			set
			{
				if ((this._timeStamp != value))
				{
					this.OntimeStampChanging(value);
					this.SendPropertyChanging();
					this._timeStamp = value;
					this.SendPropertyChanged("timeStamp");
					this.OntimeStampChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_bfPlanckId", DbType="Text", UpdateCheck=UpdateCheck.Never)]
		public string bfPlanckId
		{
			get
			{
				return this._bfPlanckId;
			}
			set
			{
				if ((this._bfPlanckId != value))
				{
					this.OnbfPlanckIdChanging(value);
					this.SendPropertyChanging();
					this._bfPlanckId = value;
					this.SendPropertyChanged("bfPlanckId");
					this.OnbfPlanckIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_bfProductId", CanBeNull=false)]
		public string bfProductId
		{
			get
			{
				return this._bfProductId;
			}
			set
			{
				if ((this._bfProductId != value))
				{
					this.OnbfProductIdChanging(value);
					this.SendPropertyChanging();
					this._bfProductId = value;
					this.SendPropertyChanged("bfProductId");
					this.OnbfProductIdChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
#pragma warning restore 1591
