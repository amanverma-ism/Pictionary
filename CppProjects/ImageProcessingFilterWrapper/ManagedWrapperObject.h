#pragma once
namespace ImageProcessingCppWrapper
{
	template<class T>
	public ref class WrapperManagedObject
	{
	protected:
		T* m_Instance;
	public:
		WrapperManagedObject(T* instance)
			: m_Instance(instance)
		{
		}
		virtual ~WrapperManagedObject()
		{
			if (m_Instance != nullptr)
			{
				delete m_Instance;
			}
		}
		!WrapperManagedObject()
		{
			if (m_Instance != nullptr)
			{
				delete m_Instance;
			}
		}
		T* GetInstance()
		{
			return m_Instance;
		}
	};
}