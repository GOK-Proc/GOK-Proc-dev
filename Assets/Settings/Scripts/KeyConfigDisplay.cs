using UnityEngine;
using UnityEngine.UI;

namespace Settings
{
	class KeyConfigDisplay: MonoBehaviour
	{
		[SerializeField] private UserSettings _settings;

		[SerializeField] private Image _button_L_L;
		[SerializeField] private Image _button_L_R;
		[SerializeField] private Image _button_L_U;
		[SerializeField] private Image _button_L_D;

		[SerializeField] private Image _button_R_L;
		[SerializeField] private Image _button_R_R;
		[SerializeField] private Image _button_R_U;
		[SerializeField] private Image _button_R_D;

		[SerializeField] private Image _button_A_U;
		[SerializeField] private Image _button_A_D;

		private Color32 _color_red = new Color32(r: 255, g: 92, b: 92, a: 255);
		private Color32 _color_blue = new Color32(r: 92, g: 92, b: 255, a: 255);

		private void OnEnable()
		{
			UpdateView();
		}

		public void UpdateView()
		{
			switch (_settings.KeyConfigId)
			{
				case KeyConfigId.Pattern1:
					_button_L_L.color = _color_blue;
					_button_L_R.color = _color_red;
					_button_L_U.color = _color_blue;
					_button_L_D.color = _color_red;

					_button_R_L.color = _color_blue;
					_button_R_R.color = _color_red;
					_button_R_U.color = _color_blue;
					_button_R_D.color = _color_red;

					_button_A_U.color = _color_blue;
					_button_A_D.color = _color_red;
					break;
				case KeyConfigId.Pattern2:
					_button_L_L.color = _color_blue;
					_button_L_R.color = _color_red;
					_button_L_U.color = _color_red;
					_button_L_D.color = _color_blue;

					_button_R_L.color = _color_blue;
					_button_R_R.color = _color_red;
					_button_R_U.color = _color_red;
					_button_R_D.color = _color_blue;

					_button_A_U.color = _color_red;
					_button_A_D.color = _color_blue;
					break;
				case KeyConfigId.Pattern3:
					_button_L_L.color = _color_blue;
					_button_L_R.color = _color_red;
					_button_L_U.color = _color_blue;
					_button_L_D.color = _color_red;

					_button_R_L.color = _color_red;
					_button_R_R.color = _color_blue;
					_button_R_U.color = _color_blue;
					_button_R_D.color = _color_red;

					_button_A_U.color = _color_blue;
					_button_A_D.color = _color_red;
					break;
				case KeyConfigId.Pattern4:
					_button_L_L.color = _color_red;
					_button_L_R.color = _color_blue;
					_button_L_U.color = _color_red;
					_button_L_D.color = _color_blue;

					_button_R_L.color = _color_blue;
					_button_R_R.color = _color_red;
					_button_R_U.color = _color_red;
					_button_R_D.color = _color_blue;

					_button_A_U.color = _color_red;
					_button_A_D.color = _color_blue;
					break;
			}
		}
	}
}