import React, { useState, useEffect, useContext } from 'react';
import { useParams } from 'react-router-dom';
import { postData, getDataWithId, putData, deleteData } from '../../Services/FetchService';
import { WorkspaceContext } from '../Side/WorkspaceContext';

const TaskComment = () => {
  const { taskId } = useParams();
  const { getInitials } = useContext(WorkspaceContext);
  const [comment, setComment] = useState('');
  const [comments, setComments] = useState([]);
  const [editingCommentId, setEditingCommentId] = useState(null); // Track the comment being edited
  const [editedComment, setEditedComment] = useState(''); // Track the content of the edited comment


  const handleInputChange = (e) => {
    setComment(e.target.value);
  };

  const handleEditedInputChange = (e) => {
    setEditedComment(e.target.value);
  };


  const getComments = async () => {
    try {
      const fetchedComments = await getDataWithId(`http://localhost:5157/backend/comment/GetCommentsByTaskId?taskId`, taskId);
      const sortedComments = fetchedComments.data.sort(
        (a, b) => new Date(b.dateAdded) - new Date(a.dateAdded)
      );
      setComments(sortedComments);
    } catch (error) {
      console.error('Error fetching comments: ', error);
    }
  };

  useEffect(() => {
    getComments();
  }, [taskId]);



  const handleSave = async () => {
    try {
      if (comment.trim()) {
        await postData('http://localhost:5157/backend/comment/CreateComment',{ content: comment, taskId });
        setComment('');
        getComments();
      }
    } catch (error) {
      console.error('Error creating a comment: ', error);
    }
  };


  const handleSaveEdit = async (commentId) => {
    try {
      if (editedComment.trim()) {
        await putData('http://localhost:5157/backend/comment/UpdateComment',{ commentId, content: editedComment });
        setEditingCommentId(null);
        setEditedComment('');
        getComments();
      }
    } catch (error) {
      console.error('Error editing comment: ', error);
    }
  };



  const handleDelete = async (commentId) => {
    try {
      await deleteData('http://localhost:5157/backend/comment/DeleteComment',{ commentId });
      getComments();
    } catch (error) {
      console.error('Error deleting comment: ', error);
    }
  };

  const handleCancelEdit = () => {
    setEditingCommentId(null);
    setEditedComment('');
  };

  return (
    <div>
      <div>
        <textarea
          value={comment}
          onChange={handleInputChange}
          placeholder="Write a comment..."
          className="bg-gray-900 bg-opacity-50 p-2 rounded-md resize-none focus:outline-none focus:ring-2 focus:ring-blue-500 overflow-hidden w-[490px] h-[40px]"
        />
        <div className="flex justify-start space-x-2 mt-2">
          <button
            className="bg-blue-500 hover:bg-blue-600 text-gray-800 font-semibold rounded px-3 py-1"
            onClick={handleSave}
          >
            Save
          </button>
        </div>
      </div>

      <div className="mt-4 border-solid border-gray-500 border-opacity-45 border p-2 rounded-md">
        <h3 className="text-lg font-semibold mb-2 bg-gray-900 pl-3 py-1 rounded-sm bg-opacity-25">Comments</h3>
        <ul className="space-y-4">
          {comments.length > 0 ? (
            comments.map((commentItem) => (
              <li key={commentItem.commentId} className="bg-gray-800 p-3 rounded-lg flex flex-col space-y-2">
                <div className="flex items-center">
                  <div className="flex-shrink-0 w-9 h-9 rounded-full flex items-center justify-center text-sm text-white bg-gradient-to-r from-orange-400 to-orange-600">
                    {getInitials(commentItem.firstName, commentItem.lastName)}
                  </div>
                  <span className="ml-2 font-semibold text-gray-400">
                    {commentItem.firstName} {commentItem.lastName}
                  </span>
                  <span className="text-gray-500 text-xs ml-2">
                    Commented on: {new Date(commentItem.dateAdded).toLocaleString()}
                  </span>
                </div>


                {editingCommentId === commentItem.commentId ? (
                  <div>
                    <textarea
                      value={editedComment}
                      onChange={handleEditedInputChange}
                      className="bg-gray-900 bg-opacity-50 p-2 rounded-md resize-none focus:outline-none focus:ring-2 focus:ring-blue-500 w-full"
                    />
                    <div className="flex justify-start space-x-2 mt-2">
                      <button
                        className="bg-blue-500 hover:bg-blue-600 text-gray-800 font-semibold rounded px-3 py-1"
                        onClick={() => handleSaveEdit(commentItem.commentId)}
                      >
                        Save
                      </button>
                      <button
                        className="bg-gray-800 hover:bg-slate-700 text-gray-400 font-semibold rounded px-3 py-1"
                        onClick={handleCancelEdit}
                      >
                        Cancel
                      </button>
                    </div>
                  </div>
                ) : (
                  <div className="flex justify-between">
                    <p className="mt-1 text-gray-400">{commentItem.content}</p>
                    <div className="flex space-x-3 text-xs">
                      <button
                        className="hover:text-gray-300"
                        onClick={() => {
                          setEditingCommentId(commentItem.commentId);
                          setEditedComment(commentItem.content);
                        }}
                      >
                        <u>Edit</u>
                      </button>
                      <button
                        className="hover:text-gray-300"
                        onClick={() => handleDelete(commentItem.commentId)}
                      >
                        <u>Delete</u>
                      </button>
                    </div>
                  </div>
                )}
              </li>
            ))
          ) : (
            <p className="text-gray-400">No comments yet.</p>
          )}
        </ul>
      </div>
    </div>
  );
};

export default TaskComment;
