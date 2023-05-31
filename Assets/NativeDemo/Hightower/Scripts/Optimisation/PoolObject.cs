using UnityEngine;
using System.Collections;
namespace PubScale.SdkOne.NativeAds.Hightower
{
	public class PoolObject : MonoBehaviour
	{
		public virtual void OnObjectReuse()
		{
			this.StopAllCoroutines();
		}

		public void Destroy()
		{
			gameObject.SetActive(false);
		}
		public IEnumerator Destroy(float time)
		{
			yield return new WaitForSeconds(time);
			gameObject.SetActive(false);
		}
	}
}