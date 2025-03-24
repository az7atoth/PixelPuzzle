using System;
using System.Collections.Generic;
using UnityEngine;

namespace PixelPuzzle
{
	[CreateAssetMenu(fileName = "SFX_Config", menuName = "Scriptable Objects/SFX_Config")]
	public class SFX_Config : ScriptableObject
	{
		public IReadOnlyCollection<SFX_Setting> Settings => _settings;

		[SerializeField] private SFX_Setting[] _settings;

		[Serializable]
		public class SFX_Setting
		{
			[field: SerializeField] public SoundKey Key;
			[field: SerializeField] public AudioClip[] AudioClips;
		}
	} 
}
