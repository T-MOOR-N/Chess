using System;

namespace Chess.Moves
{

	/// <summary>
	/// ����������� ����� ���������� ����.
	/// </summary>
	public abstract class CMove
	{
		/// <summary>
		/// �������� ��������, ����������� �������� �� ���.
		/// </summary>
		public bool IsDone { get; private set; }

		/// <summary>
		/// ������ �������� � ���������� ���.
		/// </summary>
		protected Action DoAction;

		/// <summary>
		/// �������� �������� � ������ ����.
		/// </summary>
		protected Action UndoAction;

		/// <summary>
		/// ��������� ���.
		/// </summary>
		public void Do()
		{
			if (IsDone)
			{
				return;
			}

			DoAction();
			IsDone = true;
		}

		/// <summary>
		/// �������� ���.
		/// </summary>
		public void Undo()
		{
			if (!IsDone)
			{
				return;
			}

			UndoAction();
			IsDone = false;
		}
	}
}