const LimitModal = ({onClose})=>(

    <div className="fixed z-30 inset-0 flex justify-center items-center transition-colors bg-black/20">
        <div className="bg-white rounded-xl shadow p-6 text-center">
            <p className="text-red-500 font-bold text-l">You cannot create more than 10 boards in this workspace.</p>
            <button
                onClick={onClose}
                className="mt-4 bg-gray-800 font-bold text-white px-4 py-2 rounded-md hover:text-white hover:bg-gray-900 transition-colors duration-300 ease-in-out">
                OK
            </button>
        </div>
    </div>
);
export default LimitModal