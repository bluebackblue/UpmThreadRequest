

/**
	Copyright (c) blueback
	Released under the MIT License
	@brief Request。リスト。
*/


/** BlueBack.Request
*/
namespace BlueBack.Request
{
	/** ThreadRequest_List
	*/
	public sealed class ThreadRequest_List<ITEM> : System.IDisposable
		where ITEM : class
	{
		/** [cache]core
		*/
		private ThreadRequest_Core<ITEM> core;

		/** list
		*/
		private System.Collections.Generic.Queue<ITEM> list;

		/** lockobject
		*/
		private object lockobject;

		/** constructor
		*/
		public ThreadRequest_List(ThreadRequest_Core<ITEM> a_core)
		{
			//core
			this.core = a_core;

			//list
			this.list = new System.Collections.Generic.Queue<ITEM>();

			//lockobject
			this.lockobject = new object();
		}

		/** [System.IDisposable]破棄。
		*/
		public void Dispose()
		{
			//lockobject
			this.lockobject = null;

			//[cache]core
			this.core = null;

			//list
			if(this.list != null){
				this.list.Clear();
				this.list = null;
			}
		}

		/** 設定。
		*/
		public void Enqueue(ITEM a_item)
		{
			//Enqueue
			lock(this.lockobject){
				this.list.Enqueue(a_item);
			}

			//Wakeup
			this.core.Wakeup();
		}

		/** 取得。

			return == null : データなし。

		*/
		public ITEM Dequeue()
		{
			lock(this.lockobject){
				if(this.list.Count > 0){
					return this.list.Dequeue();
				}
			}

			return null;
		}

		/** GetCount
		*/
		public int GetCount()
		{
			lock(this.lockobject){
				return this.list.Count;
			}
		}
	}
}

