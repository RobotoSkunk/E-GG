using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utils {
	class Time {
		public static float PowerDeltaTime() {
			return UnityEngine.Time.deltaTime / UnityEngine.Time.fixedDeltaTime;
		}

		public static int FixedFrameCount() {
			return Mathf.RoundToInt(UnityEngine.Time.fixedTime / UnityEngine.Time.fixedDeltaTime);
		}
	}

	class Math {
		public static Vector2 GetDirVector2D(float angle) {
			return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
		}
		public static float Direction(Vector2 pos1, Vector2 pos2) {
			return Mathf.Atan2(pos2.y - pos1.y, pos2.x - pos1.x);
		}

		public static IEnumerable<double> Arange(double start, int count) {
			return Enumerable.Range((int)start, count).Select(v => (double)v);
		}
		public static List<double> LinSpace(double maxAngle, int numberOfBullets) {
			var result = new List<double>();
			if (numberOfBullets % 2 == 1) {
				result.Add(0.0d);
			}
			if (numberOfBullets <= 1) {
				return result;
			}
			var step = (2 * maxAngle) / (double)(numberOfBullets - 1);
			for (int i = 1; i < (numberOfBullets / 2) + 1; i++) {
				result.Add(i * step);
				result.Add(-i * step);
			}
			return result;
		}

		/*public static IEnumerable<double> LinSpace(double maxAngle, int num, bool endpoint = true) {
			var result = new List<double>();
			if (num <= 0) {
				return result;
			}

			if (endpoint) {
				if (num == 1) {
					return new List<double>() { -maxAngle };
				}

				var step = (2 * maxAngle) / (num - 1.0d);
				result = Arange(0, num).Select(v => (v * step) - maxAngle).ToList();
			} else {
				var step = (2 * maxAngle) / num;
				result = Arange(0, num).Select(v => (v * step) - maxAngle).ToList();
			}

			return result;
		}*/
	}
}
