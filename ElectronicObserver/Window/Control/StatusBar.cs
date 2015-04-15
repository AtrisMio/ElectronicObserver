﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ElectronicObserver.Window.Control {
	public partial class StatusBar : UserControl {


		#region Property

		private int _value;
		[Browsable( true )]
		[DefaultValue( 66 )]
		public int Value {
			get { return _value; }
			set {
				_value = value;
				if ( !_usePrevValue )
					_prevValue = _value;
				Refresh();
			}
		}

		private int _prevValue;
		[Browsable( true )]
		[DefaultValue( 88 )]
		public int PrevValue {
			get { return _prevValue; }
			set {
				if ( _usePrevValue )
					_prevValue = value;
				Refresh();
			}
		}

		private int _maximumValue;
		[Browsable( true )]
		[DefaultValue( 100 )]
		public int MaximumValue {
			get { return _maximumValue; }
			set {
				_maximumValue = value;
				Refresh();
			}
		}

		private bool _usePrevValue;
		[Browsable( true )]
		[DefaultValue( true )]
		public bool UsePrevValue {
			get { return _usePrevValue; }
			set {
				_usePrevValue = value;
				if ( !_usePrevValue )
					_prevValue = _value;
				Refresh();
			}
		}


		private Color _barColor0;
		[Browsable( true )]
		[DefaultValue( typeof( Color ), "255, 0, 0" )]
		public Color BarColor0 {
			get { return _barColor0; }
			set {
				_barColor0 = value;
				Refresh();
			}
		}

		private Color _barColor1;
		[Browsable( true )]
		[DefaultValue( typeof( Color ), "255, 136, 0" )]
		public Color BarColor1 {
			get { return _barColor1; }
			set {
				_barColor1 = value;
				Refresh();
			}
		}

		private Color _barColor2;
		[Browsable( true )]
		[DefaultValue( typeof( Color ), "255, 204, 0" )]
		public Color BarColor2 {
			get { return _barColor2; }
			set {
				_barColor2 = value;
				Refresh();
			}
		}

		private Color _barColor3;
		[Browsable( true )]
		[DefaultValue( typeof( Color ), "0, 204, 0" )]
		public Color BarColor3 {
			get { return _barColor3; }
			set {
				_barColor3 = value;
				Refresh();
			}
		}

		private Color _barColor4;
		[Browsable( true )]
		[DefaultValue( typeof( Color ), "0, 68, 204" )]
		public Color BarColor4 {
			get { return _barColor4; }
			set {
				_barColor4 = value;
				Refresh();
			}
		}

		private Color _barColorIncrement;
		[Browsable( true )]
		[DefaultValue( typeof( Color ), "136, 34, 34" )]
		public Color BarColorIncrement {
			get { return _barColorIncrement; }
			set {
				_barColorIncrement = value;
				Refresh();
			}
		}

		private Color _barColorDecrement;
		[Browsable( true )]
		[DefaultValue( typeof( Color ), "68, 255, 0" )]
		public Color BarColorDecrement {
			get { return _barColorDecrement; }
			set {
				_barColorDecrement = value;
				Refresh();
			}
		}

		private Color _barColorBackground;
		[Browsable( true )]
		[DefaultValue( typeof( Color ), "68, 68, 68" )]
		public Color BarColorBackground {
			get { return _barColorBackground; }
			set {
				_barColorBackground = value;
				Refresh();
			}
		}


		private int _barThickness;
		[Browsable( true )]
		[DefaultValue( 4 )]
		public int BarThickness {
			get { return _barThickness; }
			set {
				_barThickness = value;
				Refresh();
			}
		}

		private int _barBackgroundOffset;
		[Browsable( true )]
		[DefaultValue( 1 )]
		public int BarBackgroundOffset {
			get { return _barBackgroundOffset; }
			set {
				_barBackgroundOffset = value;
				Refresh();
			}
		}


		#endregion



		public StatusBar() {
			InitializeComponent();

			SetStyle( ControlStyles.ResizeRedraw, true );

			_value = 66;
			_prevValue = 88;
			_maximumValue = 100;
			_usePrevValue = true;
			
			_barColor0 = FromArgb( 0xFFFF0000 );
			_barColor1 = FromArgb( 0xFFFF8800 );
			_barColor2 = FromArgb( 0xFFFFCC00 );
			_barColor3 = FromArgb( 0xFF00CC00 );
			_barColor4 = FromArgb( 0xFF0044CC );
			_barColorIncrement = FromArgb( 0xFF44FF00 );
			_barColorDecrement = FromArgb( 0xFF882222 );
			_barColorBackground = FromArgb( 0xFF888888 );

			_barThickness = 4;
			_barBackgroundOffset = 1;
			
		}



		private double GetPercentage( int value, int max ) {
			if ( max <= 0 || value <= 0 )
				return 0.0;
			else if ( value > max )
				return 1.0;
			else
				return (double)value / max;
		}

		private Color GetBarColor() {
			if ( Value <= MaximumValue * 0.25 )
				return BarColor0;
			else if ( Value <= MaximumValue * 0.50 )
				return BarColor1;
			else if ( Value <= MaximumValue * 0.75 )
				return BarColor2;
			else if ( Value < MaximumValue )
				return BarColor3;
			else
				return BarColor4;
		}


		private Color FromArgb( uint color ) {
			return Color.FromArgb( unchecked( (int)color ) );
		}


		private void StatusBar_Paint( object sender, PaintEventArgs e ) {

			Graphics g = e.Graphics;
			Rectangle basearea = new Rectangle( Padding.Left, Padding.Top, Width - Padding.Horizontal, Height - Padding.Vertical );

			//HP Bar

			using ( var b = new SolidBrush( BarColorBackground ) ) {
				g.FillRectangle( b, new Rectangle( basearea.X + BarBackgroundOffset, basearea.Bottom - BarThickness, basearea.Width - BarBackgroundOffset, BarThickness ) );
			}
			using ( var b = new SolidBrush( Value > PrevValue ? BarColorIncrement : BarColorDecrement ) ) {
				g.FillRectangle( b, new Rectangle( basearea.X, basearea.Bottom - BarThickness - BarBackgroundOffset,
					(int)Math.Ceiling( ( basearea.Width - BarBackgroundOffset ) * GetPercentage( Math.Max( Value, PrevValue ), MaximumValue ) ), BarThickness ) );
			}
			using ( var b = new SolidBrush( GetBarColor() ) ) {
				g.FillRectangle( b, new Rectangle( basearea.X, basearea.Bottom - BarThickness - BarBackgroundOffset,
					(int)Math.Ceiling( ( basearea.Width - BarBackgroundOffset ) * GetPercentage( Math.Min( Value, PrevValue ), MaximumValue ) ), BarThickness ) );
			}

		}


		public override Size GetPreferredSize( Size proposedSize ) {
			return new Size( Math.Max( Size.Width, BarThickness + BarBackgroundOffset + Padding.Horizontal ), 
				BarThickness + BarBackgroundOffset + Padding.Vertical );
		}

	}
}
