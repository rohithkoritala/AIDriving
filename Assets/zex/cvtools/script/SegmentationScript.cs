using UnityEngine;
using System.Collections.Generic;

namespace zex.cvtools
{
	[RequireComponent(typeof(Camera))]
	public class SegmentationScript : MLView {

		[SerializeField, HideInInspector]
		public Shader shader=null;
		[SerializeField, HideInInspector]
		private bool m_enable = true;
		[SerializeField, HideInInspector]
		private KeyCode m_onoffkey = KeyCode.None;
		private MaterialPropertyBlock m_propertyBlock = null;
		public RenderTexture renderTexture;

		public bool enable
		{
			get{ return m_enable; }
			set{ 
				m_enable = value; 
				if(view != null)
				{
					//view.enabled = enable;
				}
			}
		}

		public KeyCode enablekey
		{
			get {return m_onoffkey; }
			set {m_onoffkey = value; }
		}

		private void Awake() {
			InitCamera();
			shader = Shader.Find ("Hidden/SegmentationShader");

			renderTexture = new RenderTexture(new RenderTextureDescriptor(64,64, RenderTextureFormat.Default, 16));
			view.targetTexture = renderTexture;
		}

		// Use this for initialization
		void Start () {
			view.clearFlags = CameraClearFlags.Color;
			view.backgroundColor = Color.black;

			// set segmentation shader as replacement shader
			view.SetReplacementShader (shader, "");

			// initialize property block
			m_propertyBlock = new MaterialPropertyBlock();

			UpdateMaterialPropertyBlock ();

			base.Start();
		}

		// Update is called once per frame
		void Update () {
			base.Update();
			UpdateMaterialPropertyBlock ();
		}

		public override IEnumerable<float> GetMLInput() {
			//UpdateMaterialPropertyBlock();
			return base.GetMLInput();
		}

		void UpdateMaterialPropertyBlock(){
			var renderers = GameObject.FindObjectsOfType<Renderer> ();

			foreach (var r in renderers) {
				var tag = r.gameObject.tag;
				m_propertyBlock.SetColor ("_ObjectColor", TagsManager.GetColor (tag));
				r.SetPropertyBlock (m_propertyBlock);
			}
		}

		void OnDisable()
		{
			enable = false;
		}

		void OnEnable()
		{
			enable = true;
		}
	}

}